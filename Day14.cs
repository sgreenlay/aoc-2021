using System.Diagnostics;
using System.IO;
using System.Linq;
using Utilities;

class Day14 : IDay
{
    class Input
    {
        public string Polymer;
        public Dictionary<char, Dictionary<char, char>> Instructions;

        public static Input Parse(IEnumerable<string> input)
        {
            var iter = input.GetEnumerator();

            while (iter.MoveNext() && iter.Current == "") {}

            string polymer = iter.Current;

            while (iter.MoveNext() && iter.Current == "") {}

            Dictionary<char, Dictionary<char, char>> instructions = new Dictionary<char, Dictionary<char, char>>();
            do {
                var instruction = iter.Current.Split(" -> ");
                var start = instruction.First().First();
                var end = instruction.First().Last();
                var insert = instruction.Last().First();

                if (instructions.ContainsKey(start))
                {
                    instructions[start].Add(end, insert);
                }
                else
                {
                    instructions.Add(start, new Dictionary<char, char>() { { end, insert } });
                }
            } while (iter.MoveNext() && iter.Current != "");

            return new Input {
                Polymer = polymer,
                Instructions = instructions,
            };
        }
    }

    Int64 runSteps(Input input, int steps)
    {
        string polymer = input.Polymer;

        for (var step = 0; step < steps; ++step)
        {
            List<char> nextPolymer = new List<char>();

            var start = polymer.GetEnumerator();
            var end = polymer.GetEnumerator();

            end.MoveNext();

            while (start.MoveNext() && end.MoveNext())
            {
                nextPolymer.Add(start.Current);
                if (input.Instructions.ContainsKey(start.Current))
                {
                    var ends = input.Instructions[start.Current];
                    if (ends.ContainsKey(end.Current))
                    {
                        nextPolymer.Add(ends[end.Current]);
                    }
                }
            }
            nextPolymer.Add(start.Current);

            polymer = new string(nextPolymer.ToArray());
        }
        
        var counts = polymer.CharCounts();
        var mostCommon = counts.MaxBy(kv => kv.Value);
        var leastCommon = counts.MinBy(kv => kv.Value);

        return mostCommon.Value - leastCommon.Value;
    }
    
    Int64 part1(Input input)
    {
        return runSteps(input, 10);
    }

    Int64 part2(Input input)
    {
        return runSteps(input, 40);
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