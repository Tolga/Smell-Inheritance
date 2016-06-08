namespace Smell_Inheritance.Models
{
    public class Smelly
    {
        public string Name { get; set; }
        public bool Status { get; set; }

        public enum Inheritances
        {
            NONE,
            Both,
            Subclass,
            Superclass
        }
    }
}
