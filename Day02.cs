using System.Diagnostics;
using System.IO;
using System.Linq;
using Utilities;

class Day02 : IDay
{
    struct Input {
        public int FirstConstraint;
        public int SecondConstraint;
        public char Character;
        public string Password;

        public static Input Parse(string s) {
            // Format: #-# c: str

            string[] sections = s.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            
            int[] constraints = sections[0].Split('-').Select(s => int.Parse(s)).ToArray();
            char character = sections[1].First();
            string password = sections[2];

            return new Input {
                FirstConstraint = constraints[0],
                SecondConstraint = constraints[1],
                Character = character,
                Password = password,
            };
        }
    }
    
    int part1(IEnumerable<Input> inputs)
    {
        return inputs.Count(input => {
            var characterCount = input.Password.Count(c => c == input.Character);
            return characterCount >= input.FirstConstraint && characterCount <= input.SecondConstraint;
        });
    }

    int part2(IEnumerable<Input> inputs)
    {
        return inputs.Count(input => 
                input.Password[input.FirstConstraint - 1] == input.Character ^
                input.Password[input.SecondConstraint - 1] == input.Character);
    }

    public void run()
    {
        var input = File.ReadAllText(@"input/day02.txt").SplitOnNewLine().Select(s => Input.Parse(s));

        var testInput = @"
1-3 a: abcde
1-3 b: cdefg
2-9 c: ccccccccc".SplitOnNewLine().Select(s => Input.Parse(s));

        Debug.Assert(part1(testInput) == 2);
        Debug.Assert(part2(testInput) == 1);

        Console.WriteLine($"Part 1: {part1(input)}");
        Console.WriteLine($"Part 2: {part2(input)}");
    }
}