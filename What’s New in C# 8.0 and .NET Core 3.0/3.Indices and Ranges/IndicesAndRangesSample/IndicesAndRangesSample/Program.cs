using System;
using System.Linq;

namespace IndicesAndRangesSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var numbers = Enumerable.Range(1, 10).ToArray();
            var copy = numbers[0..^0];
            var allButFirst = numbers[1..];
            var lastItemRange = numbers[^1..];
            var lastItem = numbers[^1];
            var lastThree = numbers[^3..];

            Index middle = 4;
            Index threeFromEnd = ^3;
            Range customRange = middle..threeFromEnd;
            var custom = numbers[customRange];

            Console.WriteLine($"numbers: {string.Join(", ", numbers)}");
            Console.WriteLine($"copy: {string.Join(", ", copy)}");
            Console.WriteLine($"allButFirst: {string.Join(", ", allButFirst)}");
            Console.WriteLine($"lastItemRange: {string.Join(", ", lastItemRange)}");
            Console.WriteLine($"lastItem: {lastItem}");
            Console.WriteLine($"lastThree: {string.Join(", ", lastThree)}");
            Console.WriteLine($"customRange: {customRange}");
            Console.WriteLine($"custom: {string.Join(", ", custom)}");


        }
    }
}
