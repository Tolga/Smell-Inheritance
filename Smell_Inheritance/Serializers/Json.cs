using System.Collections.Generic;
using System.IO;
using System.Linq;
using Smell_Inheritance.Models;

namespace Smell_Inheritance.Serializers
{
    public class Json
    {
        private List<string> Data { get; }

        public Json()
        {
            Data = new List<string>();
        }

        public void AddUser(string userName)
        {
            if (!Data.Any())
                Data.Add("{\"Users\" : {");
            Data.Add("{\"User\" : \"" + userName + "\",");
            Data.Add("\"Projects\" : [");
        }

        /*public void Add(int projectId, List<Commit> commits)
        {
            Data.Add("{\"ProjectId\" : \"" + projectId + "\",");
            Data.Add("\"Commits\" : [");

            foreach (var commit in commits)
            {
                Data.Add("{\"CommitId\" : \"" + commit.CommitId + "\", " + "\"TimeStamp\" : \"" + commit.Date + "\", \"Changes\" : [");
                    Data.Add("{\"ChangeType\" : \"Added\", " + "\"Quantity\" : \"" +commit.Changes.Added + "\"},");
                    Data.Add("{\"ChangeType\" : \"Modified\", " + "\"Quantity\" : \"" +commit.Changes.Modified + "\"},");
                    Data.Add("{\"ChangeType\" : \"Deleted\", " + "\"Quantity\" : \"" +commit.Changes.Deleted + "\"},");
                Data.Add("]}");
            }

            Data.Add("]}");
        }*/

        public void CloseUser()
        {
            Data.Add("]}");
        }

        public void CloseFile()
        {
            Data.Add("}}");
        }

        public void Save(string fileName = "Commits")
        {
            File.WriteAllLines(fileName + ".json", Data);
        }
    }
}
