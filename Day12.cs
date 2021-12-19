using System.Diagnostics;
using System.IO;
using System.Linq;
using Utilities;

class Day12 : IDay
{
    class Edge
    {
        public string Start;
        public string End;

        public static Edge Parse(string line)
        {
            var ends = line.Split("-");
            return new Edge {
                Start = ends[0],
                End = ends[1],
            };
        }
    }

    class Map
    {
        Dictionary<string, HashSet<string>> Nodes;

        public IEnumerable<List<string>> Paths(string start, string end, bool revisitOne)
        {
            List<List<string>> paths = new List<List<string>>();

            Stack<(List<string>, Dictionary<string, int>)> horizon = new Stack<(List<string>, Dictionary<string, int>)>();
            horizon.Push((new List<string>() { "start" }, new Dictionary<string, int>() { { "start", 1 } }));
            
            while (horizon.Count > 0)
            {
                var horizonEdge = horizon.Pop();

                var location = horizonEdge.Item1.Last();
                var adjacents = Nodes[location];

                foreach (var adjacent in adjacents)
                {
                    if (adjacent.Equals("start"))
                    {
                        continue;
                    }
                    
                    var newPath = horizonEdge.Item1.Concat(new List<string>() { adjacent }).ToList();
                    
                    if (adjacent.Equals("end"))
                    {
                        paths.Add(newPath);
                    }
                    else if (char.IsUpper(adjacent.First()))
                    {
                        horizon.Push((newPath, horizonEdge.Item2));
                    }
                    else if (!horizonEdge.Item2.ContainsKey(adjacent) || 
                             (revisitOne && !horizonEdge.Item2.ContainsValue(2)))
                    {
                        var newVisited = new Dictionary<string, int>(horizonEdge.Item2);

                        if (newVisited.ContainsKey(adjacent))
                        {
                            newVisited[adjacent]++;
                        }
                        else
                        {
                            newVisited.Add(adjacent, 1);
                        }

                        horizon.Push((newPath, newVisited));
                    }
                }
            }
            return paths;
        }
        
        public static Map FromEdges(IEnumerable<Edge> edges)
        {
            Dictionary<string, HashSet<string>> nodes = new Dictionary<string, HashSet<string>>();

            foreach (var edge in edges)
            {
                if (!nodes.ContainsKey(edge.Start))
                {
                    nodes.Add(edge.Start, new HashSet<string>() { edge.End });
                }
                else
                {
                    nodes[edge.Start].Add(edge.End);
                }

                if (!nodes.ContainsKey(edge.End))
                {
                    nodes.Add(edge.End, new HashSet<string>() { edge.Start });
                }
                else
                {
                    nodes[edge.End].Add(edge.Start);
                }
            }

            return new Map {
                Nodes = nodes
            };
        }
    }
    
    int part1(IEnumerable<Edge> input)
    {
        return Map.FromEdges(input).Paths("start", "end", false).Count();
    }

    int part2(IEnumerable<Edge> input)
    {
        return Map.FromEdges(input).Paths("start", "end", true).Count();
    }

    public void run()
    {
        var testInput1 = @"
start-A
start-b
A-c
A-b
b-d
A-end
b-end
".SplitOnNewLine().Select(Edge.Parse);

        Debug.Assert(part1(testInput1) == 10);
        Debug.Assert(part2(testInput1) == 36);

        var testInput2 = @"
dc-end
HN-start
start-kj
dc-start
dc-HN
LN-dc
HN-end
kj-sa
kj-HN
kj-dc
".SplitOnNewLine().Select(Edge.Parse);

        Debug.Assert(part1(testInput2) == 19);
        Debug.Assert(part2(testInput2) == 103);

        var testInput3 = @"
fs-end
he-DX
fs-he
start-DX
pj-DX
end-zg
zg-sl
zg-pj
pj-he
RW-he
fs-DX
pj-RW
zg-RW
start-pj
he-WI
zg-he
pj-fs
start-RW
".SplitOnNewLine().Select(Edge.Parse);

        Debug.Assert(part1(testInput3) == 226);
        Debug.Assert(part2(testInput3) == 3509);

        var input = File.ReadAllText(@"input/day12.txt").SplitOnNewLine().Select(Edge.Parse);

        Console.WriteLine($"Part 1: {part1(input)}");
        Console.WriteLine($"Part 2: {part2(input)}");
    }
}