using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Utilities;

class Day04 : IDay
{
    struct BingoBoard : ICloneable
    {
        public int[][] Grid;

        public static BingoBoard Parse(IEnumerator<string> iter)
        {
            List<int[]> rows = new List<int[]>();
            var rx = new Regex(@"\s+", RegexOptions.Compiled);
            do {
                rows.Add(rx.Split(iter.Current).Select(int.Parse).ToArray());
            } while (iter.MoveNext() && iter.Current != "");
            return new BingoBoard {
                Grid = rows.ToArray(),
            };
        }

        public void Mark(int n)
        {
            var dimension = Grid.Length;
            for (var r = 0; r < dimension; ++r)
            {
                for (var c = 0; c < dimension; ++c)
                {
                    if (Grid[r][c] == n)
                    {
                        Grid[r][c] = -1;
                    }
                }
            }
        }

        public bool IsWinner()
        {
            var dimension = Grid.Length;
            
            for (var r = 0; r < dimension; ++r)
            {
                var row = Grid[r];
                if (row.Count(i => i == -1) == dimension)
                {
                    return true;
                }
            }

            for (var c = 0; c < dimension; ++c)
            {
                var column = Grid.Select(row => row[c]).ToArray();
                if (column.Count(i => i == -1) == dimension)
                {
                    return true;
                }
            }

            return false;
        }

        public int Score()
        {
            int score = 0;

            var dimension = Grid.Length;
            for (var r = 0; r < dimension; ++r)
            {
                for (var c = 0; c < dimension; ++c)
                {
                    if (Grid[r][c] != -1)
                    {
                        score += Grid[r][c];
                    }
                }
            }

            return score;
        }

        public object Clone()
        {
            return new BingoBoard {
                Grid = (int[][])Grid.Clone(),
            };
        }
    }
    
    struct Bingo
    {
        public IEnumerable<int> Numbers;
        public IEnumerable<BingoBoard> Boards;

        public static Bingo Parse(IEnumerable<string> input)
        {
            var iter = input.GetEnumerator();

            while (iter.MoveNext() && iter.Current == "") {}

            var numbers = iter.Current.Split(',').Select(int.Parse);
            iter.MoveNext();

            List<BingoBoard> boards = new List<BingoBoard>();
            while (iter.MoveNext())
            {
                boards.Add(BingoBoard.Parse(iter));
            }
            
            return new Bingo {
                Numbers = numbers,
                Boards = boards,
            };
        }
    }

    int part1(Bingo bingo)
    {
        List<BingoBoard> boards = bingo.Boards.Select(b => (BingoBoard)b.Clone()).ToList();

        foreach (var n in bingo.Numbers)
        {
            for (var i = 0; i < boards.Count; ++i)
            {
                boards[i].Mark(n);
            }
            
            var winners = bingo.Boards.Where(board => board.IsWinner());
            if (winners.Count() != 0)
            {
                return n * winners.First().Score();
            }
        }
        throw new ArgumentException("No winners");
    }

    int part2(Bingo bingo)
    {
        List<BingoBoard> boards = bingo.Boards.Select(b => (BingoBoard)b.Clone()).ToList();

        List<int> winningScores = new List<int>();
        foreach (var n in bingo.Numbers)
        {
            for (var i = 0; i < boards.Count; ++i)
            {
                boards[i].Mark(n);
            }

            winningScores = winningScores.Concat(boards.Where(board => board.IsWinner()).Select(board => n * board.Score())).ToList();
            boards = boards.Where(board => !board.IsWinner()).ToList();
        }
        return winningScores.Last();
    }

    public void run()
    {
        var testInput = Bingo.Parse(@"
7,4,9,5,11,17,23,2,0,14,21,24,10,16,13,6,15,25,12,22,18,20,8,19,3,26,1

22 13 17 11  0
 8  2 23  4 24
21  9 14 16  7
 6 10  3 18  5
 1 12 20 15 19

 3 15  0  2 22
 9 18 13 17  5
19  8  7 25 23
20 11 10 24  4
14 21 16 12  6

14 21 17 24  4
10 16 15  9 19
18  8 23 26 20
22 11 13  6  5
 2  0 12  3  7
".SplitOnNewLine(false));

        Debug.Assert(part1(testInput) == 4512);
        Debug.Assert(part2(testInput) == 1924);

        var input = Bingo.Parse(File.ReadAllText(@"input/day04.txt").SplitOnNewLine(false));

        Console.WriteLine($"Part 1: {part1(input)}");
        Console.WriteLine($"Part 2: {part2(input)}");
    }
}