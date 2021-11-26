using System.Diagnostics;
using System.IO;
using System.Linq;
using Utilities;

class Day03 : IDay
{
    struct Map
    {
        private string[] input;
        private int width => input[0].Length;
        private int height => input.Length;

        public char Get(int x, int y)
        {
            return input[y][x % width];
        }

        public int CountTreesForSlope(int dx, int dy)
        {
            int x = 0;
            int y = 0;

            int treeCount = 0;

            while (y < height)
            {
                if (Get(x, y) == '#')
                {
                    treeCount++;
                }

                x += dx;
                y += dy;
                
            }

            return treeCount;
        }

        public static Map FromInput(string s)
        {
            return new Map
            {
                input = s.SplitOnNewLine().ToArray(),
            };
        }
    }

    int part1(Map input)
    {
        return input.CountTreesForSlope(3, 1);
    }

    int part2(Map input)
    {
        return input.CountTreesForSlope(1, 1) *
               input.CountTreesForSlope(3, 1) *
               input.CountTreesForSlope(5, 1) *
               input.CountTreesForSlope(7, 1) *
               input.CountTreesForSlope(1, 2);
    }

    public void run()
    {
        var input = Map.FromInput(File.ReadAllText(@"input/day03.txt"));

        var testInput = Map.FromInput(@"
..##.......
#...#...#..
.#....#..#.
..#.#...#.#
.#...##..#.
..#.##.....
.#.#.#....#
.#........#
#.##...#...
#...##....#
.#..#...#.#");

        Debug.Assert(part1(testInput) == 7);
        Debug.Assert(part2(testInput) == 336);

        Console.WriteLine($"Part 1: {part1(input)}");
        Console.WriteLine($"Part 2: {part2(input)}");
    }
}