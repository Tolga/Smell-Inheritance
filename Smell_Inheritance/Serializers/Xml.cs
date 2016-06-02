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

        public void AddUser(string userName)
        {
            if (!Data.Any())
                Data.Add("<Users>");
            Data.Add("<User>");
            Data.Add("<Name>" + userName + "</Name>");
            Data.Add("<Projects>");
        }

        /*public void Add(int projectId, List<Commit> commits)
        {
            Data.Add("<Project>");
                Data.Add("<ProjectId>" + projectId + "</ProjectId>");
                Data.Add("<Commits>");
                    foreach (var commit in commits)
                    {
                        Data.Add("<Commit>");
                            Data.Add("<CommitId>" + commit.CommitId + "</CommitId>");
                            Data.Add("<TimeStamp>" + commit.Date + "</TimeStamp>");
                            Data.Add("<Changes>");
                                Data.Add("<Added>" + commit.Changes.Added + "</Added>");
                                Data.Add("<Modified>" + commit.Changes.Modified + " </Modified>");
                                Data.Add("<Deleted>" + commit.Changes.Deleted + "</Deleted>");
                            Data.Add("</Changes>");
                        Data.Add("</Commit>");
                    }
                Data.Add("</Commits>");
            Data.Add("</Project>");
        }*/

        public void CloseUser()
        {
            Data.Add("</Projects>");
            Data.Add("</User>");
        }

        public void CloseFile()
        {
            Data.Add("</Users>");
        }

        public void Save(string fileName = "Commits")
        {
            File.WriteAllLines(fileName + ".xml", Data);
        }
    }
}
