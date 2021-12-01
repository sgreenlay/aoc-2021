using System.Diagnostics;
using System.IO;
using System.Linq;
using Utilities;

class Day01 : IDay
{
    int part1(int[] input)
    {
        var increase = 0;
        var prev = input[0];
        for (var i = 1; i < input.Length; ++i)
        {
            if (prev < input[i])
            {
                increase++;
            }
            prev = input[i];
        }
        return increase;
    }

    int part2(int[] input)
    {
        int[] windows = new int[input.Length - 2];
        for (var i = 0; i < input.Length - 2; ++i)
        {
            windows[i] = input[i] + input[i+1] + input[i+2];
        }
        return part1(windows);
    }

    public void run()
    {
        var input = File.ReadAllText(@"input/day01.txt").SplitOnNewLine().Select(s => int.Parse(s)).ToArray();

        int[] testInput = { 199, 200, 208, 210, 200, 207, 240, 269, 260, 263 };

        Debug.Assert(part1(testInput) == 7);
        Debug.Assert(part2(testInput) == 5);

        Console.WriteLine($"Part 1: {part1(input)}");
        Console.WriteLine($"Part 2: {part2(input)}");
    }
}