using System.Collections.Generic;

namespace MaxSearchAlg
{

    internal class MaxComparer : IComparer<int>
    {
        public int Compare(int x, int y)
        {
            return (x >= y) ? -1 : 1;
        }
    }

    internal static class StackExtensions
    {
        internal static bool IsEmpty<TValue>(this Stack<TValue> stack)
        {
            return stack.Count == 0;
        }
    }

    internal static class Extensions
    {
        internal static bool IsEmpty<TKey, TValue>(this SortedList<TKey, TValue> sortedList)
        {
            return sortedList.Count == 0;
        }

        internal static void Add(this SortedList<int, INode> sortedList, INode node)
        {
            sortedList.Add(node.f, node);
        }

        internal static INode Pop(this SortedList<int, INode> sortedList)
        {
            var top = sortedList.Values[0];
            sortedList.RemoveAt(0);
            return top;
        }
    }
}
