using System.Collections.Generic;

namespace Smell_Inheritance.Models
{
    public class SmellyClass
    {
        public string Name { get; set; }
        public Dictionary<string, bool> Smells { get; set; } = new Dictionary<string, bool>() { { "Blob Class", false }, { "Blob Operation", false }, { "Data Class", false }, { "Data Clumps", false }, { "Distorted Hierarchy", false }, { "External Duplication", false }, { "Feature Envy", false }, { "God Class", false }, { "Intensive Coupling", false }, { "Internal Duplication", false }, { "Message Chains", false }, { "Refused Parent Bequest", false }, { "Schizophrenic Class", false }, { "Shotgun Surgery", false }, { "Sibling Duplication", false }, { "Tradition Breaker", false }, { "NA", false } };
        public List<Relation> Relations { get; set; }
        public ClassType Type { get; set; }

        public enum ClassType
        {
            SuperClass,
            SubClass
        }

        public enum SmellOn
        {
            NONE,
            Both,
            Subclass,
            Superclass
        }
    }
}
