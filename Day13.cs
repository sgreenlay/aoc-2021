using System.Diagnostics;
using System.IO;
using System.Linq;
using Utilities;

class Day13 : IDay
{
    class Input
    {
        public HashSet<(int, int)> Points;
        public List<(char, int)> Folds;

        public static Input Parse(IEnumerable<string> input)
        {
            var iter = input.GetEnumerator();

            while (iter.MoveNext() && iter.Current == "") {}

            HashSet<(int, int)> points = new HashSet<(int, int)>();
            do {
                var row = iter.Current.Split(',').Select(int.Parse);
                points.Add((row.First(), row.Last()));
            } while (iter.MoveNext() && iter.Current != "");

            while (iter.MoveNext() && iter.Current == "") {}

            List<(char, int)> folds = new List<(char, int)>();
            do {
                var instruction = iter.Current.Split(' ').Last().Split('=');
                folds.Add((instruction.First().First(), int.Parse(instruction.Last())));
            } while (iter.MoveNext() && iter.Current != "");

            return new Input {
                Points = points,
                Folds = folds,
            };
        }
    }

    HashSet<(int, int)> DoFold(HashSet<(int, int)> points, (char, int) fold)
    {
        IEnumerable<(int, int)> remaining;
        IEnumerable<(int, int)> folding;

        if (fold.Item1 == 'x')
        {
            remaining = points.Where(p => p.Item1 <= fold.Item2);
            folding = points.Where(p => p.Item1 > fold.Item2).Select(p => (fold.Item2 - (p.Item1 - fold.Item2), p.Item2));
        }
        else if (fold.Item1 == 'y')
        {
            remaining = points.Where(p => p.Item2 <= fold.Item2);
            folding = points.Where(p => p.Item2 > fold.Item2).Select(p => (p.Item1, fold.Item2 - (p.Item2 - fold.Item2)));
        }
        else
        {
            throw new ArgumentException("Invalid axis");
        }

        return remaining.Union(folding).ToHashSet();
    }
    
    int part1(Input input)
    {
        return DoFold(input.Points, input.Folds.First()).Count();
    }

    void part2(Input input)
    {
        HashSet<(int, int)> points = input.Points;
        foreach (var fold in input.Folds)
        {
            points = DoFold(points, fold);
        }

        var xs = points.Select(p => p.Item1);
        var minX = xs.Min();
        var maxX = xs.Max();
        
        var ys = points.Select(p => p.Item1);
        var minY = ys.Min();
        var maxY = ys.Max();

        for (var y = minY; y <= maxY; ++y)
        {
            for (var x = minX; x <= maxX; ++x)
            {
                if (points.Contains((x, y)))
                {
                    Console.Write("#");
                }
                else
                {
                    Console.Write(" ");
                }
            }
            Console.WriteLine();
        }
    }

    public void run()
    {
        var testInput = Input.Parse(@"
6,10
0,14
9,10
0,3
10,4
4,11
6,0
6,12
4,1
0,13
10,12
3,4
3,0
8,4
1,10
2,14
8,10
9,0

fold along y=7
fold along x=5
".SplitOnNewLine(false));

        Debug.Assert(part1(testInput) == 17);
        part2(testInput);

        var input = Input.Parse(File.ReadAllText(@"input/day13.txt").SplitOnNewLine(false));

        Console.WriteLine($"Part 1: {part1(input)}");
        Console.WriteLine($"Part 2:");
        part2(input);
    }
}