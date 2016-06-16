using System.Collections.Generic;
using System.IO;
using System.Linq;
using Smell_Inheritance.Processors;

namespace Smell_Inheritance
{
    internal class Program
    {
        private static List<string> Rules => File.ReadAllLines("Rules.txt").ToList();
        private static List<string> ReadReferences => File.ReadAllLines("Smell_Reference.csv").ToList();
        private static List<string> ReadInheritances => File.ReadAllLines("Smell_Inheritance.csv").ToList();
        private static List<string> Smells => File.ReadAllText("Smells.txt").Split(char.Parse(",")).ToList();

        private static void Main()
        {
            var ruleParser = new RuleParser();
            ruleParser.Process(Rules);
            ruleParser.Save("Reference_Rules");

            /*
            var smells = new SmellParser();
            var smellyProjects = smells.Process(ReadReferences);
            smells.Save(Smells, smellyProjects, "Referenced_Smells");
            */

            //new Serialize(commits);
        }
    }
}