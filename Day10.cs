using System.Diagnostics;
using System.IO;
using System.Linq;
using Utilities;

class Day10 : IDay
{
    (Stack<char>, char) ProcessLine(string line)
    {
        var stack = new Stack<char>();
        foreach (var ch in line)
        {
            switch (ch)
            {
                case ')':
                    if (stack.Peek() == '(')
                    {
                        stack.Pop();
                    }
                    else
                    {
                        return (stack, ch);
                    }
                    break;
                case ']':
                    if (stack.Peek() == '[')
                    {
                        stack.Pop();
                    }
                    else
                    {
                        return (stack, ch);
                    }
                    break;
                case '}':
                    if (stack.Peek() == '{')
                    {
                        stack.Pop();
                    }
                    else
                    {
                        return (stack, ch);
                    }
                    break;
                case '>':
                    if (stack.Peek() == '<')
                    {
                        stack.Pop();
                    }
                    else
                    {
                        return (stack, ch);
                    }
                    break;
                case '(':
                case '[':
                case '{':
                case '<':
                    stack.Push(ch);
                    break;
                default:
                    throw new ArgumentException("Invalid input");
            }
        }
        return (stack, '\n');
    }
    
    Int64 part1(IEnumerable<string> input)
    {
        Int64 points = 0;
        foreach (var line in input)
        {
            var (_, ch) = ProcessLine(line);
            switch (ch) {
                case ')': points += 3; break;
                case ']': points += 57; break;
                case '}': points += 1197; break;
                case '>': points += 25137; break;
                default: break;
            }
        }
        return points;
    }

    Int64 part2(IEnumerable<string> input)
    {
        var points = new List<Int64>();
        foreach (var line in input)
        {
            var (stack, ch) = ProcessLine(line);
            switch (ch) {
                case ')': break;
                case ']': break;
                case '}': break;
                case '>': break;
                default:
                    Int64 score = 0;
                    while (stack.Count > 0)
                    {
                        score *= 5;
                        switch (stack.Pop())
                        {
                            case '(': score += 1; break;
                            case '[': score += 2; break;
                            case '{': score += 3; break;
                            case '<': score += 4; break;
                            default:
                                throw new ArgumentException("Invalid input");
                        }
                    }
                    points.Add(score);
                    break;
            }
        }
        points.Sort();
        return points[points.Count / 2];
    }

    public void run()
    {
        var testInput = @"
[({(<(())[]>[[{[]{<()<>>
[(()[<>])]({[<{<<[]>>(
{([(<{}[<>[]}>{[]{[(<()>
(((({<>}<{<{<>}{[]{[]{}
[[<[([]))<([[{}[[()]]]
[{[{({}]{}}([{[{{{}}([]
{<[[]]>}<{[{[{[]{()[[[]
[<(<(<(<{}))><([]([]()
<{([([[(<>()){}]>(<<{{
<{([{{}}[<[[[<>{}]]]>[]]
".SplitOnNewLine();

        Debug.Assert(part1(testInput) == 26397);
        Debug.Assert(part2(testInput) == 288957);

        var input = File.ReadAllText(@"input/day10.txt").SplitOnNewLine();

        Console.WriteLine($"Part 1: {part1(input)}");
        Console.WriteLine($"Part 2: {part2(input)}");
    }
}