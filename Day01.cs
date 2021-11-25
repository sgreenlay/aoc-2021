using System.Diagnostics;
using System.IO;
using System.Linq;
using Utilities;

class Day01 : IDay
{
    int part1(IEnumerable<int> input)
    {
        var result = input.CombinationsOfLength(2).First((pair) => pair[0] + pair[1] == 2020);
        return result[0] * result[1];
    }

    int part2(IEnumerable<int> input)
    {
        var result = input.CombinationsOfLength(3).First((tuple) => tuple[0] + tuple[1] + tuple[2] == 2020);
        return result[0] * result[1] * result[2];
    }

    public void run()
    {
        var input = File.ReadAllText(@"input/day01.txt").Split(
            new string[] { "\r\n", "\r", "\n" },
            System.StringSplitOptions.RemoveEmptyEntries
        ).Select(s => int.Parse(s));

        var testInput = new int[]{ 1721, 979, 366, 299, 675, 1456 };
        Debug.Assert(part1(testInput) == 514579);
        Debug.Assert(part2(testInput) == 241861950);

        Console.WriteLine($"Part 1: {part1(input)}");
        Console.WriteLine($"Part 2: {part2(input)}");
    }
}