using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Smell_Inheritance.Models;
using Smell_Inheritance.Processors;
//using Smell_Inheritance.Serializers;

namespace Smell_Inheritance
{
    internal class Program
    {
        private static List<string> ReadInheritances => File.ReadAllLines("Smell_Inheritance.csv").ToList();
        private static List<string> Smells => File.ReadAllText("Smells.txt").Split(char.Parse(",")).ToList();

        private static void Main()
        {
            var output = new List<string>();
            var smellyProjects = new InheritedSmells().Process(ReadInheritances);

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

                    foreach (var smell in Smells)
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

                            foreach (var smell in Smells)
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
                    foreach (var smell in Smells)
                    {
                        if (subSmells.Any(s => s.Key == smell && s.Value) &&
                            superSmells.Any(b => b.Key == smell && b.Value))
                        {
                            newLine += "," + Smelly.Inheritances.Both + "." + smell;
                        }
                        else if (subSmells.Any(s => s.Key == smell && s.Value) &&
                                 !superSmells.Any(b => b.Key == smell && b.Value))
                        {
                            newLine += "," + Smelly.Inheritances.Subclass + "." + smell;
                        }
                        else if (!subSmells.Any(s => s.Key == smell && s.Value) &&
                                 superSmells.Any(b => b.Key == smell && b.Value))
                        {
                            newLine += "," + Smelly.Inheritances.Superclass + "." + smell;
                        } /*
                        else
                        {
                            newLine += ",?";
                        }*/
                    }

                    output.Add(newLine);
                }
            }
            /*
            smells = smells.Distinct().ToList();
            smells.Sort();
            File.WriteAllText("Smells.txt", string.Join(",", smells));
            Console.WriteLine("Sublasses: {0} - Superclasses {1}", totalSubClass, totalSuperClass);
            */
            /*
            var header = "Subclass," + string.Join(",", Smells);
            output.Insert(0, header);
            */
            File.WriteAllLines("Inherited_Smells.csv", output);
            Console.ReadLine();
        }

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