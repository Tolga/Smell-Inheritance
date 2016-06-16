using System.Collections.Generic;
using Smell_Inheritance.Serializers;

namespace Smell_Inheritance.Processors
{
    public class Serializer
    {
        /*
        public Serialize(HashSet<User> commits, string fileName = "Commits")
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
