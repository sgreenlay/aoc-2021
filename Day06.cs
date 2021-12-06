using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using Utilities;

class Day06 : IDay
{
    Int64 runSimulation(IEnumerable<int> inputs, int days)
    {
        var state = inputs.Select(i => (i, (Int64)1)).ToArray();
        for (var d = 0; d < days; ++d)
        {
            Int64 statesToAdd = 0;
            for (var i = 0; i < state.Length; ++i)
            {
                if (state[i].Item1 == 0)
                {
                    state[i].Item1 = 7;
                    statesToAdd += state[i].Item2;
                }
                state[i].Item1 -= 1;
            }
            if (statesToAdd > 0)
            {
                state = state.Append((8, statesToAdd)).ToArray();
            }
        }
        var count =  state.Select(state => (Int64)state.Item2).Sum();
        return count;
    }

    Int64 part1(IEnumerable<int> inputs)
    {
        return runSimulation(inputs, 80);
    }

    Int64 part2(IEnumerable<int> inputs)
    {
        return runSimulation(inputs, 256);
    }

    public void run()
    {
        var testInput = @"3,4,3,1,2".Split(",").Select(int.Parse);

        Debug.Assert(part1(testInput) == 5934);
        Debug.Assert(part2(testInput) == 26984457539);

        var input = File.ReadAllText(@"input/day06.txt").Split(",").Select(int.Parse);

        Console.WriteLine($"Part 1: {part1(input)}");
        Console.WriteLine($"Part 2: {part2(input)}");
    }
}