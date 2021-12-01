using System.Diagnostics;
using System.IO;
using System.Linq;
using Utilities;

class Day01 : IDay
{
    int part1(IEnumerable<int> input)
    {
        return input.Window(2).Count(w => w.First() < w.Last());
    }

    int part2(IEnumerable<int> input)
    {
        return part1(input.Window(3).Select(w => w.Sum()));
    }

    public void run()
    {
        var input = File.ReadAllText(@"input/day01.txt").SplitOnNewLine().Select(s => int.Parse(s));

        int[] testInput = { 199, 200, 208, 210, 200, 207, 240, 269, 260, 263 };

        Debug.Assert(part1(testInput) == 7);
        Debug.Assert(part2(testInput) == 5);

        Console.WriteLine($"Part 1: {part1(input)}");
        Console.WriteLine($"Part 2: {part2(input)}");
    }
}