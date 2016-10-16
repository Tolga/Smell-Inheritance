using System.Collections.Generic;
using System.IO;
using System.Linq;
using Smell_Inheritance.Models;

namespace Smell_Inheritance.Serializers
{
    public class Xml
    {
        private List<string> Data { get; set; }

        public Xml()
        {
            Data = new List<string>();
        }

        public void AddProject(string projectName)
        {
            if (!Data.Any())
                Data.Add("<Projects>");
            Data.Add("<Project>");
            Data.Add("<Name>" + projectName + "</Name>");
            Data.Add("<Classes>");
        }

        public void Add(string className, List<Relation> relations, Dictionary<string, bool> smells, SmellyClass.ClassType type)
        {
            Data.Add("<Class>");
                Data.Add("<Name>" + className + "</Name>");
                Data.Add("<RelatedClasses>");
                    foreach (var relation in relations)
                    {
                        Data.Add("<RelatedClass>");
                            Data.Add("<ProjectName>" + relation.ProjectName + "</ProjectName>");
                            Data.Add("<ClassName>" + relation.ClassName + "</ClassName>");
                        Data.Add("</RelatedClass>");
                    }
                Data.Add("</RelatedClasses>");
                Data.Add("<Smells>");
                    foreach (var smell in smells)
                    {
                        Data.Add("<Smell>" + smell.Key + "</Smell>");
                    }
                Data.Add("</Smells>");
                Data.Add("<Type>" + type + "</Type>");
            Data.Add("</Class>");
        }

        public void CloseProject()
        {
            Data.Add("</Classes>");
            Data.Add("</Project>");
        }

        public void CloseFile()
        {
            Data.Add("</Projects>");
        }

        public void Save(string fileName = "MappedData")
        {
            File.WriteAllLines(fileName + ".xml", Data);
        }
    }
}
