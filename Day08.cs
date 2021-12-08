using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Utilities;

class Day08 : IDay
{
    struct Signals
    {
        public IEnumerable<string> Inputs;
        public IEnumerable<string> Outputs;

        public static Signals Parse(string input)
        {
            var inputsAndOutputs = input.Split("|");
            return new Signals {
                Inputs = inputsAndOutputs.First().SplitOnWhitespace(),
                Outputs = inputsAndOutputs.Last().SplitOnWhitespace(),
            };
        }
    }

    /*
      0:      1:      2:      3:      4:
    aaaa    ....    aaaa    aaaa    ....
    b    c  .    c  .    c  .    c  b    c
    b    c  .    c  .    c  .    c  b    c
    ....    ....    dddd    dddd    dddd
    e    f  .    f  e    .  .    f  .    f
    e    f  .    f  e    .  .    f  .    f
    gggg    ....    gggg    gggg    ....

      5:      6:      7:      8:      9:
    aaaa    aaaa    aaaa    aaaa    aaaa
    b    .  b    .  .    c  b    c  b    c
    b    .  b    .  .    c  b    c  b    c
    dddd    dddd    ....    dddd    dddd
    .    f  e    f  .    f  e    f  .    f
    .    f  e    f  .    f  e    f  .    f
    gggg    gggg    ....    gggg    gggg

    0 uses 6
    1 uses 2 -
    2 uses 5
    3 uses 5
    4 uses 4 -
    5 uses 5
    6 uses 6
    7 uses 3 -
    8 uses 7 -
    9 uses 6
    */
    
    int part1(IEnumerable<Signals> signals)
    {
        return signals.Select(s => s.Outputs.Count(s =>
            s.Length == 2 || s.Length == 3 || s.Length == 4 || s.Length == 7
        )).Sum();
    }

    int part2(IEnumerable<Signals> signals)
    {
        return signals.Select(s => {
            var one = s.Inputs.Where(i => i.Length == 2).First();
            var four = s.Inputs.Where(i => i.Length == 4).First();
            var seven = s.Inputs.Where(i => i.Length == 3).First();
            var eight = s.Inputs.Where(i => i.Length == 7).First();

            // 9 fully overlaps 4, 0 and 6 do not
            var nine = s.Inputs.Where(i => i.Length == 6 && i.Intersect(four).Count() == 4).First();
            // 0 fully overlaps 7, 6 does not
            var zero = s.Inputs.Where(i => i.Length == 6 && i != nine && i.Intersect(seven).Count() == 3).First();
            // 6 is the remaining input of length 6
            var six = s.Inputs.Where(i => i.Length == 6 && i != nine && i != zero).First();

            // 3 full overlaps 1, but 2 and 5 do not
            var three = s.Inputs.Where(i => i.Length == 5 && i.Intersect(one).Count() == 2).First();
            // 5 fully overlaps 9, 2 does not
            var five = s.Inputs.Where(i => i.Length == 5 && i != three && i.Intersect(nine).Count() == 5).First();
            // 2 is the remaining input of length 5
            var two = s.Inputs.Where(i => i.Length == 5 && i != three && i != five).First();

            var mapping = new string[] { zero, one, two, three, four, five, six, seven, eight, nine };
            return s.Outputs.Select(o => Array.FindIndex(mapping, m => m.AnagramOf(o))).ToInt();
        }).Sum();
    }

    public void run()
    {
        var testInput1 = @"
acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab | cdfeb fcadb cdfeb cdbaf
".SplitOnNewLine().Select(Signals.Parse);

        Debug.Assert(part1(testInput1) == 0);
        Debug.Assert(part2(testInput1) == 5353);

        var testInput2 = @"
be cfbegad cbdgef fgaecd cgeb fdcge agebfd fecdb fabcd edb | fdgacbe cefdb cefbgd gcbe
edbfga begcd cbg gc gcadebf fbgde acbgfd abcde gfcbed gfec | fcgedb cgb dgebacf gc
fgaebd cg bdaec gdafb agbcfd gdcbef bgcad gfac gcb cdgabef | cg cg fdcagb cbg
fbegcd cbd adcefb dageb afcb bc aefdc ecdab fgdeca fcdbega | efabcd cedba gadfec cb
aecbfdg fbg gf bafeg dbefa fcge gcbea fcaegb dgceab fcbdga | gecf egdcabf bgf bfgea
fgeab ca afcebg bdacfeg cfaedg gcfdb baec bfadeg bafgc acf | gebdcfa ecba ca fadegcb
dbcfg fgd bdegcaf fgec aegbdf ecdfab fbedc dacgb gdcebf gf | cefg dcbef fcge gbcadfe
bdfegc cbegaf gecbf dfcage bdacg ed bedf ced adcbefg gebcd | ed bcgafe cdgba cbgef
egadfb cdbfeg cegd fecab cgb gbdefca cg fgcdab egfdb bfceg | gbdfcae bgc cg cgb
gcafb gcf dcaebfg ecagb gf abcdeg gaef cafbge fdbac fegbdc | fgae cfgab fg bagce
".SplitOnNewLine().Select(Signals.Parse);

        Debug.Assert(part1(testInput2) == 26);
        Debug.Assert(part2(testInput2) == 61229);

        var input = File.ReadAllText(@"input/day08.txt").SplitOnNewLine().Select(Signals.Parse);

        Console.WriteLine($"Part 1: {part1(input)}");
        Console.WriteLine($"Part 2: {part2(input)}");
    }
}