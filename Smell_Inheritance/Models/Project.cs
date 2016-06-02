using System.Collections.Generic;

namespace Smell_Inheritance.Models
{
    public class Project
    {
        public string Name { get; set; }
        public List<SmellyClass> SmellyClass { get; set; }
    }
}
