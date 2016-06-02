using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Smell_Inheritance.Models;
using Smell_Inheritance.Processors;
using Smell_Inheritance.Serializers;

namespace Smell_Inheritance
{
    internal class Program
    {
        private static List<string> ReadInheritances => File.ReadAllLines("Smell_Inheritance.txt").ToList();
        private static List<string> Smells => File.ReadAllText("Smells.txt").Split(char.Parse(",")).ToList();

        private static void Main()
        {
            var output = new List<string>();
            var smells = new List<string>();
            var smellyProjects = new InheritedSmells().Process(ReadInheritances);

            foreach (var smellyProject in smellyProjects)
            {
                /*
                totalSubClass += smellyProject.SmellyClass.Count(sp => sp.Type == SmellyClass.Types.SubClass);
                totalSuperClass += smellyProject.SmellyClass.Count(sp => sp.Type == SmellyClass.Types.SuperClass);
                */

                foreach (var smellyClass in smellyProject.SmellyClass.FindAll(sp=>sp.Type == SmellyClass.Types.SubClass))
                {
                    var newLine = smellyClass.Name;
                    var subCounts = new Dictionary<string, int>();
                    var superCounts = new Dictionary<string, int>();

                    foreach (var smell in Smells)
                    {
                        if (smellyClass.Smells.Any(rs => rs.Name == smell))
                        {
                            if (subCounts.ContainsKey(smell))
                                subCounts[smell] += smellyClass.Smells.Find(rs => rs.Name == smell).Counts;
                            else
                                subCounts.Add(smell, smellyClass.Smells.Find(rs => rs.Name == smell).Counts);
                        }
                    }

                    if (smellyClass.Relations.Count > 0)
                    {
                        foreach (var relation in smellyClass.Relations)
                        {
                            if (smellyProjects.Any(sp => sp.Name == relation.ProjectName && sp.SmellyClass.Any(sc=>sc.Name == relation.ClassName)))
                            {
                                var relatedSmells =
                                    smellyProjects.First(sp => sp.Name == relation.ProjectName)
                                        .SmellyClass.Find(sc => sc.Name == relation.ClassName)
                                        .Smells;
                                smells.AddRange(relatedSmells.Select(smell => smell.Name));

                                foreach (var smell in Smells)
                                {
                                    if (relatedSmells.Any(rs => rs.Name == smell))
                                    {
                                        if (superCounts.ContainsKey(smell))
                                            superCounts[smell] += relatedSmells.Find(rs => rs.Name == smell).Counts;
                                        else
                                            superCounts.Add(smell, relatedSmells.Find(rs => rs.Name == smell).Counts);
                                    }
                                }
                            }
                        }
                    }

                    foreach (var smell in Smells)
                    {
                        if (subCounts.ContainsKey(smell))
                        {
                            newLine += "," + subCounts[smell];
                        }
                        else
                        {
                            newLine += "," + 0;
                        }
                        if (superCounts.ContainsKey(smell))
                        {
                            newLine += "," + superCounts[smell];
                        }
                        else
                        {
                            newLine += "," + 0;
                        }
                    }

                    output.Add(newLine);
                }
            }
            /*
            smells = smells.Distinct().ToList();
            smells.Sort();
            File.WriteAllText("Smells.txt", string.Join(",", smells));
            */
            var header = "Subclass";
            foreach (var smell in Smells)
            {
                string shortSmell;

                var splitSmell = smell.Split(char.Parse(" ")).ToList();
                if (splitSmell.Count > 1)
                {
                    shortSmell = splitSmell[0][0] +"."+ splitSmell[1];
                }
                else
                {
                    shortSmell = splitSmell[0];
                }
                header += "," + shortSmell + "_S," + shortSmell + "_B";
            }
            output.Insert(0, header);
            //Console.WriteLine("Sublasses: {0} - Superclasses {1}", totalSubClass, totalSuperClass);
            File.WriteAllLines("Inherited_Smells.txt", output);
            Console.ReadLine();
        }
        /*
        if (smellyProject.SmellyClass.Any(sc => sc.Relations.Contains("NA")))
        {
            Console.WriteLine("YUHH");
        }*/

        /*private static void SaveCommits(HashSet<User> commits, string fileName = "Commits")
        {
            var xml = new Xml();
            var json = new Json();
            var csv = new Csv();

            foreach (var user in commits)
            {
                xml.AddUser(user.UserName);
                json.AddUser(user.UserName);
                foreach (var project in user.Projects)
                {
                    var commitsByUser = user.Commits.FindAll(c => c.ProjectId == project);
                    csv.Add(user.UserName, commitsByUser);
                    xml.Add(project, commitsByUser);
                    json.Add(project, commitsByUser);
                }
                xml.CloseUser();
                json.CloseUser();
            }

            csv.Save(fileName);
            xml.CloseFile();
            xml.Save(fileName);
            json.CloseFile();
            json.Save(fileName);
        }*/
    }
}