using System.Collections.Generic;

namespace WSMan.NET.Enumeration
{
    public static class EnumeratorExtensions
    {
        public static IEnumerable<object> Take(this IEnumerator<object> enumerator, int maximum, out bool endOfSequence)
        {
            var count = 0;
            var result = new List<object>();
            var moveNext = false;
            while (count < maximum && (moveNext = enumerator.MoveNext()))
            {
                result.Add(enumerator.Current);                
                count++;
            }
            endOfSequence = !moveNext || count < maximum;
            return result;
        }
    }
}