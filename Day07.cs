using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Utilities;

class Day07 : IDay
{
    int cheapestLocation(IEnumerable<int> inputs, Func<int, int, int> cost)
    {
        Dictionary<int, int> sumOfDistances = new Dictionary<int, int>();
        for (var i = inputs.Min(); i <= inputs.Max(); ++i)
        {
            sumOfDistances[i] = 0;
        }
        foreach (var j in inputs)
        {
            foreach (var k in sumOfDistances.Keys)
            {
                sumOfDistances[k] += cost(j, k);
            }
        }
        return sumOfDistances.Values.Min();
    }

    int part1(IEnumerable<int> inputs)
    {
        return cheapestLocation(inputs, (j, k) => Math.Abs(j - k));
    }

    int part2(IEnumerable<int> inputs)
    {
        return cheapestLocation(inputs, (j, k) => {
            var n = Math.Abs(j - k);
            return n * (n + 1) / 2;
        });
    }

    public void run()
    {
        var testInput = @"16,1,2,0,4,2,7,1,2,14".Split(',').Select(int.Parse);

        Debug.Assert(part1(testInput) == 37);
        Debug.Assert(part2(testInput) == 168);

        var input = File.ReadAllText(@"input/day07.txt").Split(',').Select(int.Parse);

        Console.WriteLine($"Part 1: {part1(input)}");
        Console.WriteLine($"Part 2: {part2(input)}");
    }
}