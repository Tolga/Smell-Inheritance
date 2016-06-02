using System.Collections.Generic;
using System.IO;
using System.Linq;
using Smell_Inheritance.Models;

namespace Smell_Inheritance.Serializers
{
    public class Csv
    {
        private List<string> Data { get; }

        public Csv()
        {
            Data = new List<string>();
        }

        /*public void Add(string userName, IList<Commit> commits)
        {
            if (!Data.Any())
            {
                Data.Add("ProjectId, CommitId, Author, TimeStamp, Added, Modified, Deleted, Total");
            }

            foreach (var commit in commits)
            {
                var total = commit.Changes.Added + commit.Changes.Modified + commit.Changes.Deleted;
                Data.Add(commit.ProjectId + ", " + commit.CommitId + ", " + userName + ", " + commit.Date + ", " + commit.Changes.Added + ", " + commit.Changes.Modified + ", " + commit.Changes.Deleted + ", " + total);
            }
        }*/

        public void Save(string fileName = "Commits")
        {
            File.WriteAllLines(fileName + ".csv", Data);
        }
    }
}
