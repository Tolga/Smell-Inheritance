using System.Collections.Generic;
using Smell_Inheritance.Serializers;
using Smell_Inheritance.Models;

namespace Smell_Inheritance.Processors
{
    public class Serializer
    {
        public void Serialize(List<Project> projects)
        {
            var xml = new Xml();
            //var json = new Json();
            //var csv = new Csv();

            foreach (var project in projects)
            {
                xml.AddProject(project.Name);
                //json.AddUser(project.Name);
                foreach (var smellyClass in project.SmellyClass)
                {
                    xml.Add(smellyClass.Name, smellyClass.Relations, smellyClass.Smells, smellyClass.Type);
                    //csv.Add(smellyClass.Name, commitsByUser);
                    //json.Add(smellyClass.Name, commitsByUser);

                }
                xml.CloseProject();
                //json.CloseUser();
            }

            //csv.Save(fileName);
            xml.CloseFile();
            xml.Save();
            //json.CloseFile();
            //json.Save(fileName);
        }
    }
}
