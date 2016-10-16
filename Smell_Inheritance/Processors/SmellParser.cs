using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Smell_Inheritance.Models;
using static Smell_Inheritance.Models.SmellyClass;
using System.Diagnostics;

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
                if (project.SmellyClass.Any(sc => sc.Type == ClassType.SubClass && sc.Name == subClass))
                {
                    // GET SUBCLASS
                    var smellySub = project.SmellyClass.Single(sc => sc.Type == ClassType.SubClass && sc.Name == subClass);
                                       
                    if (!smellySub.Smells[subSmell])
                        smellySub.Smells[subSmell] = true;

                    // ADD NAME OF RELATED CLASS IF NOT EXISTS
                    if (!smellySub.Relations.Any(r => r.ClassName.Contains(superClass)))
                    {
                        smellySub.Relations.Add(new Relation { ClassName = superClass, ProjectName = projectName });
                    }
                }
                else
                {
                    var smellyClass = new SmellyClass { Name = subClass, Relations = new List<Relation>(), Type = ClassType.SubClass};
                    smellyClass.Relations.Add(new Relation { ClassName = superClass, ProjectName = projectName });
                    smellyClass.Smells[subSmell] = true;

                    project.SmellyClass.Add(smellyClass);
                }

                // CHECK EXISTING SUPERCLASS
                if (project.SmellyClass.Any(sc => sc.Type == ClassType.SuperClass && sc.Name == superClass))
                {
                    // GET SUPERCLASS
                    var smellySuper = project.SmellyClass.Single(sc => sc.Type == ClassType.SuperClass && sc.Name == superClass);

                    if (!smellySuper.Smells[superSmell])
                        smellySuper.Smells[superSmell] = true;

                    // ADD NAME OF RELATED CLASS IF NOT EXISTS
                    if (!smellySuper.Relations.Any(r => r.ClassName.Contains(subClass)))
                    {
                        smellySuper.Relations.Add(new Relation { ClassName = subClass, ProjectName = projectName });
                    }
                }
                else
                {
                    var smellyClass = new SmellyClass { Name = superClass, Relations = new List<Relation>(), Type = ClassType.SuperClass};
                    smellyClass.Relations.Add(new Relation { ClassName = subClass, ProjectName = projectName });
                    smellyClass.Smells[superSmell] = true;

                    project.SmellyClass.Add(smellyClass);
                }
            }

            return SmellyProjects;
        }

        public void Save(List<Project> smellyProjects, string path)
        {
            var output = new List<string>();

            foreach (var smellyProject in smellyProjects)
            {
                var smellySubclassesInProject = smellyProject.SmellyClass.FindAll(sp => sp.Type == ClassType.SubClass);

                foreach (var smellySubclass in smellySubclassesInProject)
                {
                    if (smellySubclass.Name == "NA") continue;
                    var newLine = smellyProject.Name + "." + smellySubclass.Name;

                    // Creat a list of smells in all related classes
                    var relatedSmells = new List<string>();
                    foreach (var related in smellySubclass.Relations)
                    {
                        var smellyP = smellyProjects.Single(sp => sp.Name == related.ProjectName);
                        var smellyClass = smellyP.SmellyClass.Single(sc => sc.Name == related.ClassName && sc.Type == ClassType.SuperClass);
                        var smells = smellyClass.Smells.Where(pair => pair.Value == true).Select(pair => pair.Key).ToList();

                        relatedSmells.AddRange(smells);
                    }
                    relatedSmells.Distinct();

                    // Comparison of smells between Subclass and Superclass
                    foreach (var smell in smellySubclass.Smells)
                    {
                        if (relatedSmells.Contains(smell.Key))
                        {
                            if (smell.Value)
                                newLine += "," + SmellOn.Both;
                            else
                                newLine += "," + SmellOn.Superclass;
                        }
                        else if (smell.Value)
                        {
                            newLine += "," + SmellOn.Subclass;
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

            var header = "Subclass, Blob Class, Blob Operation, Data Class, Data Clumps, Distorted Hierarchy, External Duplication, Feature Envy, God Class, Intensive Coupling, Internal Duplication, Message Chains, Refused Parent Bequest, Schizophrenic Class, Shotgun Surgery, Sibling Duplication, Tradition Breaker, NA";
            output.Insert(0, header);

            File.WriteAllLines(path + ".csv", output);
        }
    }
}
