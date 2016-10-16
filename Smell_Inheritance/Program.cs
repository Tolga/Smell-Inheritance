using System.Collections.Generic;
using System.IO;
using System.Linq;
using Smell_Inheritance.Processors;

namespace Smell_Inheritance
{
    internal class Program
    {
        private static List<string> Rules => File.ReadAllLines("Rules.txt").ToList();
        //private static List<string> ReadReferences => File.ReadAllLines("Smell_Reference.csv").ToList();
        //private static List<string> ReadInheritances => File.ReadAllLines("Smell_Inheritance.csv").ToList();
        //private static List<string> Smells => File.ReadAllText("Smells.txt").Split(char.Parse(",")).ToList();

        private static void Main()
        {
            /* 
             * ASSOCIATION RULE PARSER
             */
            var ruleParser = new RuleParser();
            ruleParser.Process(Rules);
            ruleParser.Save("Inheritance_Rules");
            
            /*
            var smells = new SmellParser();
            var smellyProjects = smells.Process(ReadReferences); // MAP DATA
            smells.Save(smellyProjects, "Referenced_Smells"); // EXPORT DATA AS SPARSE MATRIX
            
            /*
             * EXPORT DATA AS XML
             * 
            var serializer = new Serializer();
            serializer.Serialize(smellyProjects);*/
        }
    }
}