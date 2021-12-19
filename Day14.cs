using System.Diagnostics;
using System.IO;
using System.Linq;
using Utilities;

class Day14 : IDay
{
    class Input
    {
        public string Polymer;
        public Dictionary<string, (char, string, string)> Instructions;

        public static Input Parse(IEnumerable<string> input)
        {
            var iter = input.GetEnumerator();

            while (iter.MoveNext() && iter.Current == "") {}

            string polymer = iter.Current;

            while (iter.MoveNext() && iter.Current == "") {}

            Dictionary<string, (char, string, string)> instructions = new Dictionary<string, (char, string, string)>();
            do {
                var instruction = iter.Current.Split(" -> ");
                var pair = instruction.First();
                var insert = instruction.Last().First();

                var first = $"{pair.First()}{insert}";
                var second = $"{insert}{pair.Last()}";

                instructions.Add(pair, (insert, first, second));
            } while (iter.MoveNext() && iter.Current != "");

            return new Input {
                Polymer = polymer,
                Instructions = instructions,
            };
        }
    }

    Int64 RunSteps(Input input, int steps)
    {
        var counts = input.Polymer.CharCounts();
        var pairs = input.Polymer.Window(2).Select(w => new string(w)).Counts();

        for (var step = 0; step < steps; ++step)
        {
            Dictionary<string, Int64> nextPairs = new Dictionary<string, Int64>();

            foreach (var pair in pairs)
            {
                if (input.Instructions.ContainsKey(pair.Key))
                {
                    var replacement = input.Instructions[pair.Key];

                    if (counts.ContainsKey(replacement.Item1))
                    {
                        counts[replacement.Item1] += pair.Value;
                    }
                    else
                    {
                        counts[replacement.Item1] = pair.Value;
                    }
                    
                    if (nextPairs.ContainsKey(replacement.Item2))
                    {
                        nextPairs[replacement.Item2] += pair.Value;
                    }
                    else
                    {
                        nextPairs[replacement.Item2] = pair.Value;
                    }

                    if (nextPairs.ContainsKey(replacement.Item3))
                    {
                        nextPairs[replacement.Item3] += pair.Value;
                    }
                    else
                    {
                        nextPairs[replacement.Item3] = pair.Value;
                    }
                }
            }

            pairs = nextPairs;
        }
        
        var mostCommon = counts.MaxBy(kv => kv.Value);
        var leastCommon = counts.MinBy(kv => kv.Value);

        return mostCommon.Value - leastCommon.Value;
    }
    
    Int64 part1(Input input)
    {
        return RunSteps(input, 10);
    }

    Int64 part2(Input input)
    {
        return RunSteps(input, 40);
    }

    public void run()
    {
        var testInput = Input.Parse(@"
NNCB

CH -> B
HH -> N
CB -> H
NH -> C
HB -> C
HC -> B
HN -> C
NN -> C
BH -> H
NC -> B
NB -> B
BN -> B
BB -> N
BC -> B
CC -> N
CN -> C
".SplitOnNewLine(false));

        Debug.Assert(part1(testInput) == 1588);
        Debug.Assert(part2(testInput) == 2188189693529);

        var input = Input.Parse(File.ReadAllText(@"input/day14.txt").SplitOnNewLine(false));

        Console.WriteLine($"Part 1: {part1(input)}");
        Console.WriteLine($"Part 2: {part2(input)}");
    }
}