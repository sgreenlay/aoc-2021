using System.Collections.Generic;

namespace Utilities
{
    public static class Numbers
    {
        // Enumerate all possible m-size combinations of [0, 1, ..., n-1] array
        // in lexicographic order (first [0, 1, 2, ..., m-1]).
        // https://codereview.stackexchange.com/a/195025
        public static IEnumerable<int[]> CombinationsOfLength(int m, int n)
        {
            if (n < m)
                throw new ArgumentException("Length can't be less than number of selected elements");
            if (n < 1)
                throw new ArgumentException("Number of selected elements can't be less than 1");
            
            int[] result = new int[m];
            Stack<int> stack = new Stack<int>(m);
            stack.Push(0);
            while (stack.Count > 0)
            {
                int index = stack.Count - 1;
                int value = stack.Pop();
                while (value < n)
                {
                    result[index++] = value++;
                    stack.Push(value);
                    if (index != m) continue;
                    yield return (int[])result.Clone();
                    break;
                }
            }
        }
    }
    
    public static class Extensions
    {
        public static IEnumerable<string> SplitOnNewLine(this string source, bool ignoreEmptyLines = true)
        {
            var options = 
                (ignoreEmptyLines ? System.StringSplitOptions.RemoveEmptyEntries : System.StringSplitOptions.None) |
                System.StringSplitOptions.TrimEntries;
            return source.Split(new string[] { "\r\n", "\r", "\n" }, options);
        }

        public static IEnumerable<string> SplitOnWhitespace(this string source, bool ignoreEmptyLines = true)
        {
            var options = 
                (ignoreEmptyLines ? System.StringSplitOptions.RemoveEmptyEntries : System.StringSplitOptions.None) |
                System.StringSplitOptions.TrimEntries;
            return source.Split(new string[] { " ", "\t" }, options);
        }

        public static int Product(this IEnumerable<int> source)
        {
            return source.Aggregate(1, (product, i) => product * i);
        }

        public static int ToInt(this char source)
        {
            if (!source.InRange('0', '9'))
                throw new ArgumentException("Must be 0 to 9");

            return source - '0';
        }

        public static bool InRange(this char source, char start, char end)
        {
            if (end < start)
                throw new ArgumentException("End must come after start");
            
            return (source >= start) && (source <= end);
        }

        public static bool InRange(this int source, int start, int end)
        {
            if (end < start)
                throw new ArgumentException("End must come after start");
            
            return (source >= start) && (source <= end);
        }

        public static Dictionary<char, Int64> CharCounts(this string str)
        {
            return str.AsEnumerable().Counts();
        }

        public static bool AnagramOf(this string a, string b)
        {
            if (a.Length != b.Length) {
                return false;
            }

            var countsA = a.CharCounts();
            var countsB = b.CharCounts();

            return countsA.Count == countsB.Count && !countsA.Except(countsB).Any();
        }

        public static int ToInt(this IEnumerable<int> source)
        {
            if (null == source)
                throw new ArgumentNullException(nameof(source));

            var length = source.Count();
            return source.Select((s, i) => {
                if (s < 0 || s > 9)
                    throw new ArgumentException("Digits must be 0 to 9");
                return s * (int)Math.Pow(10, length - i - 1);
            }).Sum();
        }

        public static Dictionary<T, Int64> Counts<T>(this IEnumerable<T> source) where T : IComparable
        {
            Dictionary<T, Int64> counts = new Dictionary<T, Int64>();
            foreach (var s in source)
            {
                if (counts.ContainsKey(s))
                {
                    counts[s] += 1;
                }
                else
                {
                    counts[s] = 1;
                }
            }
            return counts;
        }
        
        // Enumerate all possible combinations from a list of values
        // https://stackoverflow.com/a/57058345
        public static IEnumerable<T[]> Combinations<T>(this IEnumerable<T> source)
        {
            if (null == source)
                throw new ArgumentNullException(nameof(source));

            T[] data = source.ToArray();

            return Enumerable
              .Range(0, 1 << (data.Length))
              .Select(index => data
                 .Where((v, i) => (index & (1 << i)) != 0)
                 .ToArray());
        }

        // Enumerate all possible combinations of a given length from a list of values
        public static IEnumerable<T[]> CombinationsOfLength<T>(this IEnumerable<T> source, int length)
        {
            if (null == source)
                throw new ArgumentNullException(nameof(source));
            
            T[] data = source.ToArray();

            if (data.Length < length)
                throw new ArgumentException("Length can't be less than number of selected elements");
            if (length < 1)
                throw new ArgumentException("Number of selected elements can't be less than 1");

            T[] result = new T[length];
            foreach (int[] j in Numbers.CombinationsOfLength(length, data.Length))
            {
                for (int i = 0; i < length; i++)
                {
                    result[i] = data[j[i]];
                }
                yield return (T[])result.Clone();
            }
        }

        // Enumerate all sequential windows of size from a list of values
        public static IEnumerable<T[]> Window<T>(this IEnumerable<T> source, int size)
        {
            if (null == source)
                throw new ArgumentNullException(nameof(source));
            if (size < 1)
                throw new ArgumentException("Size of window can't be less than 1");
            
            Queue<T> queue = new Queue<T>(size);

            using var iter = source.GetEnumerator();
            while (queue.Count < size && iter.MoveNext())
            {
                queue.Enqueue(iter.Current);
            }

            if (queue.Count < size)
            {
                yield break;
            }

            while (iter.MoveNext())
            {
                yield return queue.ToArray();
                queue.Dequeue();
                queue.Enqueue(iter.Current);
            }
            yield return queue.ToArray();
        }
    }
}