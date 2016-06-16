using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Smell_Inheritance.Processors
{
    public class RuleParser
    {
        readonly List<string> _rules = new List<string>();

        public List<string> Process(List<string> rules)
        {
            var i = 1;
            foreach (var rule in rules)
            {
                var splits = rule.Split(new []{ "==>" }, StringSplitOptions.None);
                var left = splits[0].Trim().Split(Convert.ToChar(" ")).ToList();
                left.RemoveAt(0);
                left.RemoveAt(left.Count - 1);
                var right = splits[1].Trim().Split(new[] { "    " }, StringSplitOptions.None).ToList().First().Split(Convert.ToChar(" ")).ToList();
                right.RemoveAt(right.Count - 1);

                if (CompareClasses(left, right))
                {
                    _rules.Add(i++ + ". " + rule.Substring(1 + rule.IndexOf(Convert.ToChar("."))).Trim());
                }
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
            File.WriteAllLines(path + ".txt", _rules);
        }
    }
}
