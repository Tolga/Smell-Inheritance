using System;
using System.Collections.Generic;
using System.Linq;
using Smell_Inheritance.Models;

namespace Smell_Inheritance.Processors
{
    public class InheritedSmells
    {
        public List<Project> SmellyProjects;

        public InheritedSmells()
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
                        smellySub.Smells.Single(s => s.Name == subSmell).Counts++;
                    }
                    else
                    {
                        smellySub.Smells.Add(new Smelly { Counts = 1, Name = subSmell });
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
                    smellyClass.Smells.Add(new Smelly { Name = subSmell, Counts = 1 });

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
                        smellySuper.Smells.Single(s => s.Name == superSmell).Counts++;
                    }
                    else
                    {
                        smellySuper.Smells.Add(new Smelly { Counts = 1, Name = superSmell });
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
                    smellyClass.Smells.Add(new Smelly { Name = superSmell, Counts = 1 });

                    project.SmellyClass.Add(smellyClass);
                }
            }

            return SmellyProjects;
        }
    }
}
