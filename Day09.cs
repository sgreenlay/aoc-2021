using System.Diagnostics;
using System.IO;
using System.Linq;
using Utilities;

class Day09 : IDay
{
    struct Map
    {
        int[][] map;
        int width;
        int height;

        public int Width => width;
        public int Height => height;
        
        public int Get(int x, int y)
        {
            return map[y][x];
        }

        public void Set(int x, int y, int v)
        {
            map[y][x] = v;
        }

        public IEnumerable<(int, int)> GetAdjacent(int x, int y)
        {
            var adjacent = new List<(int, int)>();
            if (x > 0)
            {
                adjacent.Add((x-1, y));
            }
            if (x < width - 1)
            {
                adjacent.Add((x+1, y));
            }
            if (y > 0)
            {
                adjacent.Add((x, y-1));
            }
            if (y < height - 1)
            {
                adjacent.Add((x, y+1));
            }
            return adjacent;
        }

        public static Map Parse(IEnumerable<string> input)
        {
            return new Map {
                height = input.Count(),
                width = input.First().Count(),
                map = input.Select(s => s.ToCharArray().Select(ch => ch.ToInt()).ToArray()).ToArray(),
            };
        }
    }

    int part1(Map map)
    {
        List<int> lowPoints = new List<int>();
        for (var y = 0; y < map.Height; ++y)
        {
            for (var x = 0; x < map.Width; ++x)
            {
                var point = map.Get(x, y);
                if (map.GetAdjacent(x, y).Select((p) => map.Get(p.Item1, p.Item2))
                                         .Count(i => i <= point) == 0)
                {
                    lowPoints.Add(point);
                }
            }
        }
        return lowPoints.Select(i => i + 1).Sum();
    }

    int part2(Map map)
    {
        List<int> basinSizes = new List<int>();
        for (var y = 0; y < map.Height; ++y)
        {
            for (var x = 0; x < map.Width; ++x)
            {
                if (map.Get(x, y) != 9)
                {
                    map.Set(x, y, 9);

                    var basinSize = 1;
                    var adjacent = map.GetAdjacent(x, y).Where((p) => map.Get(p.Item1, p.Item2) != 9);
                    while (adjacent.Count() > 0)
                    {
                        var newAdjacent = new HashSet<(int, int)>{};
                        
                        basinSize += adjacent.Count();
                        foreach (var p in adjacent)
                        {
                            map.Set(p.Item1, p.Item2, 9);

                            var adjacentToP = map.GetAdjacent(p.Item1, p.Item2).Where((p2) => map.Get(p2.Item1, p2.Item2) != 9);
                            foreach (var p2 in adjacentToP)
                            {
                                newAdjacent.Add(p2);
                            }
                        }
                        adjacent = newAdjacent;
                    }
                    basinSizes.Add(basinSize);
                }
            }
        }
        basinSizes.Sort();
        return basinSizes.TakeLast(3).Product();
    }

    public void run()
    {
        var testInput = Map.Parse(@"
2199943210
3987894921
9856789892
8767896789
9899965678
".SplitOnNewLine());

        Debug.Assert(part1(testInput) == 15);
        Debug.Assert(part2(testInput) == 1134);

        var input = Map.Parse(File.ReadAllText(@"input/day09.txt").SplitOnNewLine());

        Console.WriteLine($"Part 1: {part1(input)}");
        Console.WriteLine($"Part 2: {part2(input)}");
    }
}