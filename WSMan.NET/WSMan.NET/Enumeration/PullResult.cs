using System.Collections.Generic;

namespace WSMan.NET.Enumeration
{
    public class PullResult
    {
        private readonly bool _endOfSequence;
        private readonly IEnumerable<object> _items;
        private readonly EnumerationMode _enumerationMode;

        public PullResult(IEnumerable<object> items, EnumerationMode enumerationMode, bool endOfSequence)
        {
            _items = items;
            _enumerationMode = enumerationMode;
            _endOfSequence = endOfSequence;
        }

        public bool EndOfSequence
        {
            get { return _endOfSequence; }
        }

        public IEnumerable<object> Items
        {
            get { return _items; }
        }

        public EnumerationMode EnumerationMode
        {
            get { return _enumerationMode; }
        }
    }
}