using System.ServiceModel.Channels;
using System.Text;
using System.Xml;

namespace WSMan.NET.WCF
{
    public class MaxEnvelopeSizeHeader : MessageHeader
    {
        public const string ElementName = "MaxEnvelopeSize";
        private readonly int _value;

        public MaxEnvelopeSizeHeader(int value)
        {
            _value = value;
        }

        protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            writer.WriteValue(_value);
        }


        public static MaxEnvelopeSizeHeader ReadFrom(XmlDictionaryReader reader)
        {
            reader.ReadStartElement(ElementName, Const.ManagementNamespace);
            var fragment = new StringBuilder();
            while (reader.NodeType == XmlNodeType.Text)
            {
                fragment.Append(reader.Value);
                reader.Read();
            }
            var result = new MaxEnvelopeSizeHeader(int.Parse(fragment.ToString()));
            reader.ReadEndElement();
            return result;
        }

        public static MaxEnvelopeSizeHeader ReadFrom(Message message)
        {
            return ReadFrom(message.Headers);
        }

        public static MaxEnvelopeSizeHeader ReadFrom(MessageHeaders messageHeaders)
        {
            MaxEnvelopeSizeHeader result;
            int index = messageHeaders.FindHeader(ElementName, Const.ManagementNamespace);
            if (index < 0)
            {
                return null;
            }
            using (XmlDictionaryReader readerAtHeader = messageHeaders.GetReaderAtHeader(index))
            {
                result = ReadFrom(readerAtHeader);
            }
            MessageHeaderInfo headerInfo = messageHeaders[index];
            if (!messageHeaders.UnderstoodHeaders.Contains(headerInfo))
            {
                messageHeaders.UnderstoodHeaders.Add(headerInfo);
            }
            return result;
        }

        public override string Name
        {
            get { return ElementName; }
        }

        public override string Namespace
        {
            get { return Const.ManagementNamespace; }
        }

        public int Value
        {
            get { return _value; }
        }
    }
}