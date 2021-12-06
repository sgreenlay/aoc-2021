using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using Utilities;

class Day06 : IDay
{
    Int64 runSimulation(IEnumerable<int> inputs, int days)
    {
        var state = inputs.ToArray();
        for (var d = 0; d < days; ++d)
        {
            var newStates = new List<int>();
            for (var i = 0; i < state.Length; ++i)
            {
                if (state[i] == 0)
                {
                    state[i] = 7;
                    newStates.Add(8);
                }
                state[i] -= 1;
            }
            state = state.Concat(newStates).ToArray();
            Console.WriteLine($"{newStates.Count} added");
        }
        return state.Length;
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