using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Smell_Inheritance.Models;

namespace Smell_Inheritance.Processors
{
    public class SmellParser
    {
        public List<Project> SmellyProjects;

        public SmellParser()
        {
            SmellyProjects = new List<Project>();
        }

        public List<Project> Process(IReadOnlyCollection<string> data)
        {
            foreach (var item in data)
            {
                var splits = item.Split(Convert.ToChar(","));
                var projectName = splits[0].Trim();
                var subClass = splits[1].Trim();
                var subSmell = splits[2].Trim();
                var superClass = splits[3].Trim();
                var superSmell = splits[4].Trim();

                Project project;

                // CHECK EXISTANCE OF PROJECT
                if (SmellyProjects.Any(i => i.Name == projectName))
                {
                    // GET THE PROJECT
                    project = SmellyProjects.Single(i => i.Name == projectName);
                }
                else
                {
                    // CREATE NEW PROJECT
                    project = new Project { Name = projectName, SmellyClass = new List<SmellyClass>() };

                    SmellyProjects.Add(project);
                }

                // CHECK EXISTING SUBCLASS
                if (project.SmellyClass.Any(sc => sc.Type == SmellyClass.Types.SubClass && sc.Name == subClass))
                {
                    // GET SUBCLASS
                    var smellySub = project.SmellyClass.Single(sc => sc.Type == SmellyClass.Types.SubClass && sc.Name == subClass);

                    // CHECK EXISTANCE OF SMELL
                    if (smellySub.Smells.Any(s => s.Name == subSmell))
                    {
                        smellySub.Smells.Single(s => s.Name == subSmell).Status = true;
                    }
                    else
                    {
                        smellySub.Smells.Add(new Smelly { Status = true, Name = subSmell });
                    }

                    // ADD NAME OF RELATED CLASS IF NOT EXISTS
                    if (!smellySub.Relations.Any(r => r.ClassName.Contains(superClass)))
                    {
                        smellySub.Relations.Add(new Relation { ClassName = superClass, ProjectName = projectName });
                    }
                }
                else
                {
                    var smellyClass = new SmellyClass { Name = subClass, Smells = new List<Smelly>(), Relations = new List<Relation>(), Type = SmellyClass.Types.SubClass};
                    smellyClass.Relations.Add(new Relation { ClassName = superClass, ProjectName = projectName });
                    smellyClass.Smells.Add(new Smelly { Name = subSmell, Status = true });

                    project.SmellyClass.Add(smellyClass);
                }

                // CHECK EXISTING SUPERCLASS
                if (project.SmellyClass.Any(sc => sc.Type == SmellyClass.Types.SuperClass && sc.Name == superClass))
                {
                    // GET SUPERCLASS
                    var smellySuper = project.SmellyClass.Single(sc => sc.Type == SmellyClass.Types.SuperClass && sc.Name == superClass);

                    // CHECK EXISTANCE OF SMELL
                    if (smellySuper.Smells.Any(s => s.Name == superSmell))
                    {
                        smellySuper.Smells.Single(s => s.Name == superSmell).Status = true;
                    }
                    else
                    {
                        smellySuper.Smells.Add(new Smelly { Status = true, Name = superSmell });
                    }

                    // ADD NAME OF RELATED CLASS IF NOT EXISTS
                    if (!smellySuper.Relations.Any(r => r.ClassName.Contains(subClass)))
                    {
                        smellySuper.Relations.Add(new Relation { ClassName = subClass, ProjectName = projectName });
                    }
                }
                else
                {
                    var smellyClass = new SmellyClass { Name = superClass, Smells = new List<Smelly>(), Relations = new List<Relation>(), Type = SmellyClass.Types.SuperClass};
                    smellyClass.Relations.Add(new Relation { ClassName = subClass, ProjectName = projectName });
                    smellyClass.Smells.Add(new Smelly { Name = superSmell, Status = true });

                    project.SmellyClass.Add(smellyClass);
                }
            }

            return SmellyProjects;
        }

        public void Save(List<string> smells, List<Project> smellyProjects, string path)
        {
            var output = new List<string>();

            foreach (var smellyProject in smellyProjects)
            {
                foreach (
                    var smellyClass in
                        smellyProject.SmellyClass.FindAll(sp => sp.Type == SmellyClass.Types.SubClass))
                {
                    if (smellyClass.Name == "NA") continue;
                    var newLine = smellyProject.Name + "." + smellyClass.Name;
                    var subSmells = new Dictionary<string, bool>();
                    var superSmells = new Dictionary<string, bool>();

                    foreach (var smell in smells)
                    {
                        // Check smells in subclass
                        if (smellyClass.Smells.Any(rs => rs.Name == smell))
                        {
                            if (subSmells.ContainsKey(smell))
                                subSmells[smell] = true;
                            else
                                subSmells.Add(smell, smellyClass.Smells.Find(rs => rs.Name == smell).Status);
                        }
                        else
                        {
                            if (!subSmells.ContainsKey(smell))
                                subSmells.Add(smell, false);
                        }
                    }

                    // Check for relations
                    if (smellyClass.Relations.Count > 0)
                    {
                        foreach (var relation in smellyClass.Relations)
                        {
                            // Get smells of related class
                            var relatedSmells =
                                smellyProjects.First(sp => sp.Name == relation.ProjectName)
                                    .SmellyClass.Find(sc => sc.Name == relation.ClassName)
                                    .Smells;

                            foreach (var smell in smells)
                            {
                                // Check smells in related class
                                if (relatedSmells.Any(rs => rs.Name == smell))
                                {
                                    // Add smells
                                    if (superSmells.ContainsKey(smell))
                                        superSmells[smell] = true;
                                    else
                                        superSmells.Add(smell, relatedSmells.Find(rs => rs.Name == smell).Status);
                                }
                                else
                                {
                                    if (!superSmells.ContainsKey(smell))
                                        superSmells.Add(smell, false);
                                }
                            }
                        }
                    }

                    // Comparison of smells between Subclass and Superclass
                    foreach (var smell in smells)
                    {
                        if (subSmells.Any(s => s.Key == smell && s.Value) &&
                            superSmells.Any(b => b.Key == smell && b.Value))
                        {
                            newLine += "," + Smelly.Inheritances.Both;
                        }
                        else if (subSmells.Any(s => s.Key == smell && s.Value) &&
                                 !superSmells.Any(b => b.Key == smell && b.Value))
                        {
                            newLine += "," + Smelly.Inheritances.Subclass;
                        }
                        else if (!subSmells.Any(s => s.Key == smell && s.Value) &&
                                 superSmells.Any(b => b.Key == smell && b.Value))
                        {
                            newLine += "," + Smelly.Inheritances.Superclass;
                        }
                        else
                        {
                            newLine += ",?";
                        }
                    }

                    output.Add(newLine);
                }
            }
            /*
            smells = smells.Distinct().ToList();
            smells.Sort();
            File.WriteAllText("smells.txt", string.Join(",", smells));
            Console.WriteLine("Sublasses: {0} - Superclasses {1}", totalSubClass, totalSuperClass);
            */

            var header = "Subclass," + string.Join(",", smells);
            output.Insert(0, header);

            File.WriteAllLines(path + ".csv", output);
        }
    }
}
