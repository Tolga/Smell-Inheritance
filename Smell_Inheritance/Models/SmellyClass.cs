using System.Collections.Generic;

namespace Smell_Inheritance.Models
{
    public class SmellyClass
    {
        public string Name { get; set; }
        public List<Smelly> Smells { get; set; }
        public List<Relation> Relations { get; set; }
        public Types Type { get; set; }

        public enum Types
        {
            SuperClass,
            SubClass
        }
    }
}
