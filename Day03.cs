using System.Diagnostics;
using System.IO;
using System.Linq;
using Utilities;

class Day03 : IDay
{
    (char, char) GetMostAndLeastCommon(char[] inputs) {
        var inputsCount = inputs.Count();
        var zeros = inputs.Count(ch => ch == '0');
        var mostcommon = (zeros > inputsCount / 2) ? '0' : '1';
        var leastcommon = (zeros > inputsCount / 2) ? '1' : '0';
        return (mostcommon, leastcommon);
    }

    int part1(IEnumerable<char[]> inputs)
    {
        var length = inputs.First().Length;

        string gamma = "";
        string epsilon = "";

        for (var i = 0; i < length; ++i) {
            var column = inputs.Select(input => input[i]).ToArray();
            var (most, least) = GetMostAndLeastCommon(column);
            gamma += most;
            epsilon += least;
        }

        return Convert.ToInt32(gamma, 2) * Convert.ToInt32(epsilon, 2);
    }

    int part2(IEnumerable<char[]> inputs)
    {
        var length = inputs.First().Length;

        var o2 = inputs;
        for (var i = 0; i < length; ++i) {
            var column = o2.Select(input => input[i]).ToArray();
            var (most, least) = GetMostAndLeastCommon(column);
            o2 = o2.Where(input => input[i] == most).ToList();
            if (o2.Count() == 1) {
                break;
            }
        }

        var co2 = inputs;
        for (var i = 0; i < length; ++i) {
            var column = co2.Select(input => input[i]).ToArray();
            var (most, least) = GetMostAndLeastCommon(column);
            co2 = co2.Where(input => input[i] == least).ToList();
            if (co2.Count() == 1) {
                break;
            }
        }

        return Convert.ToInt32(String.Concat(o2.First()), 2) * Convert.ToInt32(String.Concat(co2.First()), 2);
    }

    public void run()
    {
        var input = File.ReadAllText(@"input/day03.txt").SplitOnNewLine().Select(s => s.ToCharArray());

        var testInput = @"
00100
11110
10110
10111
10101
01111
00111
11100
10000
11001
00010
01010".SplitOnNewLine().Select(s => s.ToCharArray());

        Debug.Assert(part1(testInput) == 198);
        Debug.Assert(part2(testInput) == 230);

        Console.WriteLine($"Part 1: {part1(input)}");
        Console.WriteLine($"Part 2: {part2(input)}");
    }
}