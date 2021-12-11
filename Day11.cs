using System.Diagnostics;
using System.IO;
using System.Linq;
using Utilities;

class Day11 : IDay
{
    IEnumerable<(int, int)> GetAdjacent(int x, int y, int[][] input)
    {
        return new List<(int, int)>() {
            ( x-1, y-1 ),
            ( x  , y-1 ),
            ( x+1, y-1 ),
            ( x-1, y   ),
            ( x+1, y   ),
            ( x-1, y+1 ),
            ( x  , y+1 ),
            ( x+1, y+1 ),
        }.Where((p) => 
            p.Item1 >= 0 &&
            p.Item1 < input[0].Length &&
            p.Item2 >= 0 &&
            p.Item2 < input.Length);
    }
    
    (int[][], int) ProcessStep(int[][] input)
    {
        var flashes = 0;
        
        for (var y = 0; y < input.Length; ++y)
        {
            for (var x = 0; x < input[0].Length; ++x)
            {
                input[y][x] += 1;
            }
        }

        var modified = 0;
        do {
            modified = 0;
            for (var y = 0; y < input.Length; ++y)
            {
                for (var x = 0; x < input[0].Length; ++x)
                {
                    if (input[y][x] > 9)
                    {
                        input[y][x] = -1;
                        
                        modified++;
                        flashes++;

                        foreach (var p in GetAdjacent(x, y, input))
                        {
                            if (input[p.Item2][p.Item1] != -1)
                            {
                                input[p.Item2][p.Item1]++;
                            }
                        }
                    }
                }
            }
        } while (modified > 0);

        for (var y = 0; y < input.Length; ++y)
        {
            for (var x = 0; x < input[0].Length; ++x)
            {
                if (input[y][x] == -1)
                {
                    input[y][x] = 0;
                }
            }
        }

        /*
        for (var y = 0; y < input.Length; ++y)
        {
            for (var x = 0; x < input[0].Length; ++x)
            {
                Console.Write(input[y][x]);
            }
            Console.WriteLine();
        }
        Console.WriteLine();
        */

        return (input, flashes);
    }

    int part1(int[][] input)
    {
        int totalFlashes = 0;
        for (var step = 0; step < 100; ++step)
        {
            (input, var flashes) = ProcessStep(input);
            totalFlashes += flashes;
        }
        return totalFlashes;
    }

    int part2(int[][] input)
    {
        var step = 0;
        do {
            (input, var flashes) = ProcessStep(input);
            step++;
        } while (input.Sum(line => line.Sum()) != 0);
        return 100 + step;
    }

    public void run()
    {
        var testInput = @"
5483143223
2745854711
5264556173
6141336146
6357385478
4167524645
2176841721
6882881134
4846848554
5283751526
".SplitOnNewLine().Select(line => line.ToCharArray().Select(ch => ch.ToInt()).ToArray()).ToArray();

        Debug.Assert(part1(testInput) == 1656);
        Debug.Assert(part2(testInput) == 195);

        var input = File.ReadAllText(@"input/day11.txt")
            .SplitOnNewLine()
            .Select(line => line.ToCharArray().Select(ch => ch.ToInt()).ToArray()).ToArray();

        Console.WriteLine($"Part 1: {part1(input)}");
        Console.WriteLine($"Part 2: {part2(input)}");
    }
}