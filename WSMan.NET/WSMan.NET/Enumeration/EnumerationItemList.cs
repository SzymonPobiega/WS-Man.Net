using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace WSMan.NET.Enumeration
{
    public class EnumerationItemList : IXmlSerializable, IEnumerable<EnumerationItem>
    {
        private readonly List<EnumerationItem> _items;

        public EnumerationItemList()
        {
            _items = new List<EnumerationItem>();
        }

        public EnumerationItemList(IEnumerable<EnumerationItem> items)
        {
            _items = new List<EnumerationItem>(items);
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public bool IsEmpty
        {
            get { return _items.Count == 0; }
        }

        public void ReadXml(XmlReader reader)
        {
            if (reader.IsEmptyElement)
            {
                reader.ReadStartElement("Items", reader.NamespaceURI);
                return;
            }
            reader.ReadStartElement("Items", reader.NamespaceURI);
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                var item = new EnumerationItem();
                item.ReadXml(reader);
                _items.Add(item);
            }
            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            foreach (var item in _items)
            {
                item.WriteXml(writer);
            }
        }

        public IEnumerator<EnumerationItem> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}