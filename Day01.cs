using System.Diagnostics;
using System.IO;
using System.Linq;
using Utilities;

class Day01 : IDay
{
    int part1(IEnumerable<int> input)
    {
        return 0;
    }

    int part2(IEnumerable<int> input)
    {
        return 0;
    }

    public void run()
    {
        var input = File.ReadAllText(@"input/day01.txt").SplitOnNewLine().Select(s => int.Parse(s));

        var testInput = new int[]{ 0 };
        Debug.Assert(part1(testInput) == 0);
        Debug.Assert(part2(testInput) == 0);

        Console.WriteLine($"Part 1: {part1(input)}");
        Console.WriteLine($"Part 2: {part2(input)}");
    }
}