using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Smell_Inheritance.Processors
{
    public class RuleParser
    {
        private readonly List<string> _rules = new List<string>();

        public List<string> Process(List<string> rules)
        {
            foreach (var rule in rules)
            {
                var splits = rule.Split(new []{ "==>" }, StringSplitOptions.None);
                var left = splits[0].Trim().Split(Convert.ToChar(" ")).ToList();
                left.RemoveAt(0);
                left.RemoveAt(left.Count - 1);
                var right = splits[1].Trim().Split(new[] { "    " }, StringSplitOptions.None).ToList();
                var stats = right[1].Trim().Replace("conf", "").Replace("conv", "").Replace("lift", "").Replace("lev", "").Replace(":(", "").Replace(")", "").Replace("< ", "").Replace(">", "").Split(Convert.ToChar(" ")).ToList();
                stats.RemoveAt(3);
                right = right.First().Split(Convert.ToChar(" ")).ToList();
                right.RemoveAt(right.Count - 1);
                if (CompareClasses(left, right))
                    _rules.Add(string.Join(" ", left) + "," + string.Join(" ", right) + "," + string.Join(",", stats));
            }
            return _rules;
        }

        public bool CompareClasses(List<string> left, List<string> right)
        {
            var leftClasses = left.Select(c => c.Split(Convert.ToChar("=")).Last()).Distinct().ToList();
            var rightClasses = right.Select(c => c.Split(Convert.ToChar("=")).Last()).Distinct().ToList();
            if (leftClasses.Count != 1 || rightClasses.Count != 1) return false;
            return leftClasses.First() != rightClasses.First();
        }

        public void Save(string path)
        {
            var header = "Antecedent,Consequent,Confidence,Lift,Leverage,Conviction";
            _rules.Insert(0, header);
            File.WriteAllLines(path + ".csv", _rules);
        }
    }
}
