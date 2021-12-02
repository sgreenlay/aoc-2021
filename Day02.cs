using System.Diagnostics;
using System.IO;
using System.Linq;
using Utilities;

class Day02 : IDay
{
    enum Direction
    {
        Forward,
        Down,
        Up,
    }

    static class DirectionHelper
    {
        public static Direction Parse(string s)
        {
            switch (s)
            {
                case "forward":
                    return Direction.Forward;
                case "down":
                    return Direction.Down;
                case "up":
                    return Direction.Up;
            }
            throw new ArgumentException("Invalid direction");
        }
    }

    struct Command
    {
        public Direction Direction;
        public int Distance;

        public static Command Parse(string s)
        {
            var command = s.Split(' ');

            return new Command {
                Direction = DirectionHelper.Parse(command[0]),
                Distance = int.Parse(command[1])
            };
        }
    }

    int part1(IEnumerable<Command> input)
    {
        int x = 0;
        int y = 0;

        /*
        forward X increases the horizontal position by X units.
        down X increases the depth by X units.
        up X decreases the depth by X units.
        */

        foreach (var command in input) {
            switch (command.Direction) {
                case Direction.Down:
                    y += command.Distance;
                    break;
                case Direction.Up:
                    y -= command.Distance;
                    break;
                case Direction.Forward:
                    x += command.Distance;
                    break;
            }
        }
        return Math.Abs(x * y);
    }

    int part2(IEnumerable<Command> input)
    {
        int x = 0;
        int y = 0;
        int aim = 0;

        /*
        down X increases your aim by X units.
        up X decreases your aim by X units.
        forward X does two things:
            It increases your horizontal position by X units.
            It increases your depth by your aim multiplied by X.
        */

        foreach (var command in input) {
            switch (command.Direction) {
                case Direction.Down:
                    aim += command.Distance;
                    break;
                case Direction.Up:
                    aim -= command.Distance;
                    break;
                case Direction.Forward:
                    x += command.Distance;
                    y += aim * command.Distance;
                    break;
            }
        }
        return Math.Abs(x * y);
    }

    public void run()
    {
        var input = File.ReadAllText(@"input/day02.txt").SplitOnNewLine().Select(s => Command.Parse(s));

        var testInput = @"
forward 5
down 5
forward 8
up 3
down 8
forward 2".SplitOnNewLine().Select(s => Command.Parse(s));

        Debug.Assert(part1(testInput) == 150);
        Debug.Assert(part2(testInput) == 900);

        Console.WriteLine($"Part 1: {part1(input)}");
        Console.WriteLine($"Part 2: {part2(input)}");
    }
}