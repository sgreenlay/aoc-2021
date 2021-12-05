using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using Utilities;

class Day05 : IDay
{
    struct Vent
    {
        public Vector2 Start;
        public Vector2 End;

        public static Vent Parse(string input)
        {
            // X,Y -> X,Y
            var coordinates = input.Split(" -> ").Select(c => c.Split(",").Select(int.Parse));
            return new Vent {
                Start = new Vector2(coordinates.First().First(), coordinates.First().Last()),
                End = new Vector2(coordinates.Last().First(), coordinates.Last().Last()),
            };
        }
    }

    int countOverlaps(IEnumerable<Vent> vents, bool includeDiagonal)
    {
        Dictionary<Vector2, int> map = new Dictionary<Vector2, int>();

        foreach (var vent in vents)
        {
            int dx = (int)vent.End.X - (int)vent.Start.X;
            int dy = (int)vent.End.Y - (int)vent.Start.Y;

            if (!includeDiagonal && dx != 0 && dy != 0)
            {
                continue;
            }

            Debug.Assert(Math.Abs(dx) == Math.Abs(dy) || dx == 0 || dy == 0);

            int delta = Math.Max(Math.Abs(dx), Math.Abs(dy));
            for (int d = 0; d <= delta; ++d)
            {
                var location = new Vector2 {
                    X = vent.Start.X + (dx > 0 ? 1 : -1) * (dx != 0 ? d : 0),
                    Y = vent.Start.Y + (dy > 0 ? 1 : -1) * (dy != 0 ? d : 0)
                };

                if (map.ContainsKey(location))
                {
                    map[location]++;
                }
                else
                {
                    map[location] = 1;
                }
            }
        }
        return map.Values.Count(count => count > 1);
    }

    int part1(IEnumerable<Vent> vents)
    {
        return countOverlaps(vents, false);
    }

    int part2(IEnumerable<Vent> vents)
    {
        return countOverlaps(vents, true);
    }

    public void run()
    {
        var testInput = @"
0,9 -> 5,9
8,0 -> 0,8
9,4 -> 3,4
2,2 -> 2,1
7,0 -> 7,4
6,4 -> 2,0
0,9 -> 2,9
3,4 -> 1,4
0,0 -> 8,8
5,5 -> 8,2".SplitOnNewLine().Select(Vent.Parse);

        Debug.Assert(part1(testInput) == 5);
        Debug.Assert(part2(testInput) == 12);

        var input = File.ReadAllText(@"input/day05.txt").SplitOnNewLine().Select(Vent.Parse);

        Console.WriteLine($"Part 1: {part1(input)}");
        Console.WriteLine($"Part 2: {part2(input)}");
    }
}