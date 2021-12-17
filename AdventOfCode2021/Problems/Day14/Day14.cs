using AdventOfCode2021.Utilities;
using System.Text.RegularExpressions;

namespace AdventOfCode2021.Problems
{
    internal class Day14 : Problem
    {
        protected override string InputName => "Actual";
        private readonly Regex RuleRegex = new(@"(?<p>\S+)( -> )(?<i>\S)");

        /*
            The string is a series of pairs. 
            We can test the pairs to see if a rule applies.
            If it does, that creates two new pairs for each found pair.
            The last character never changes since we only ever insert between two characters.

            Assumptions:
                - There is an insertion rule for every combination of letters
         */

        public override object PartOne()
        {
            var input = GetInputValue();

            var template = input.First();

            var rules = GetRules(input);

            var pairs = GetInitialPairs(template);

            for (var i = 0; i < 10; i++)
            {
                pairs = GetNewPairs(pairs, rules);
            }

            var letterCounts = GetLetterCounts(pairs, template);

            return letterCounts.Max(x => x.Value) - letterCounts.Min(x => x.Value);
        }

        public override object PartTwo()
        {
            var input = GetInputValue();

            var template = input.First();

            var rules = GetRules(input);

            var pairs = GetInitialPairs(template);

            for (var i = 0; i < 40; i++)
            {
                pairs = GetNewPairs(pairs, rules);
            }

            var letterCounts = GetLetterCounts(pairs, template);

            return letterCounts.Max(x => x.Value) - letterCounts.Min(x => x.Value);
        }

        private List<Rule> GetRules(string[] input)
        {
            var rules = new List<Rule>();

            for (int i = 2; i < input.Length; i++)
            {
                var ruleMatch = RuleRegex.Match(input[i]);
                var pair = ruleMatch.Groups["p"].Value;
                var insertion = ruleMatch.Groups["i"].Value;
                rules.Add(new Rule(pair, insertion));
            }

            return rules;
        }

        private static Dictionary<string, long> GetInitialPairs(string template)
        {
            var pairs = new Dictionary<string, long>();
            for (var i = 0; i < template.Length - 1; i++)
            {
                var currentPair = template[i..(i + 2)];
                pairs.CreateOrIncrementItem(currentPair);
            }

            return pairs;
        }

        private static Dictionary<string, long> GetNewPairs(Dictionary<string, long> pairs, List<Rule> rules)
        {
            var newPairs = new Dictionary<string, long>();
            foreach (var pair in pairs)
            {
                var applicableRule = rules.SingleOrDefault(r => r.Pair == pair.Key);
                if (applicableRule != default)
                {
                    var newSet = applicableRule.GetNewPairs();

                    // Add newly created pairs counter for the number of instances of a given pair
                    newPairs.CreateOrIncrementItemByAmount(newSet[0], pair.Value);
                    newPairs.CreateOrIncrementItemByAmount(newSet[1], pair.Value);
                }
                else
                {
                    throw new Exception("Assumption failed: There is not a rule for every possible combination");
                }
            }

            return newPairs;
        }

        private static Dictionary<string, long> GetLetterCounts(Dictionary<string, long> pairs, string template)
        {
            var letterCounts = new Dictionary<string, long>();
            foreach (var pair in pairs)
            {
                // Add the number of instances of the first letter of the pair
                // The second letter will be added by the next pair
                letterCounts.CreateOrIncrementItemByAmount(pair.Key[..1], pair.Value);
            }

            // Add the last character since it will be the same as when it started
            // and it is not added when looping over the pairs. 
            letterCounts.CreateOrIncrementItem(template.Last().ToString());

            return letterCounts;
        }
    }

    internal class Rule
    {
        public string Pair { get; set; }
        public string Insertion { get; set; }

        public Rule(string pair, string insertion)
        {
            Pair = pair;
            Insertion = insertion;
        }

        public List<string> GetNewPairs()
        {
            return new List<string>()
            {
                $"{Pair[0]}{Insertion}",
                $"{Insertion}{Pair[1]}"
            };
        }
    }

    internal static class DictionaryExtensions
    {
        internal static void CreateOrIncrementItem(this Dictionary<string, long> dictionary, string newValue)
        {
            if (dictionary.ContainsKey(newValue))
            {
                dictionary[newValue]++;
            }
            else
            {
                dictionary.Add(newValue, 1);
            }
        }

        internal static void CreateOrIncrementItemByAmount(this Dictionary<string, long> dictionary, string newValue, long amount)
        {
            if (dictionary.ContainsKey(newValue))
            {
                dictionary[newValue] += amount;
            }
            else
            {
                dictionary.Add(newValue, amount);
            }
        }
    }
}
