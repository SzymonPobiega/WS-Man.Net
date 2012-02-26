using System;
using System.Collections.Generic;

namespace WSMan.NET.Enumeration
{
    public class EnumerationPullHandler : IPullHandler
    {
        private readonly IEnumerator<object> _enumerator;
        private readonly EnumerationMode _enumerationMode;

        public EnumerationPullHandler(IEnumerator<object> enumerator, EnumerationMode enumerationMode)
        {
            _enumerator = enumerator;
            _enumerationMode = enumerationMode;
        }

        public PullResult Pull(int? maxElements, TimeSpan? maxTime, string context)
        {
            bool endOfSequence;
            var items = _enumerator.Take(maxElements ?? 1, out endOfSequence);
            return new PullResult(items, _enumerationMode, endOfSequence);
        }

        public void Dispose()
        {
            // NOOP
        }
    }
}