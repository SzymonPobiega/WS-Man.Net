using System;
using System.Linq;
using System.ServiceModel;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace WSMan.NET.Enumeration
{
   public class EnumerationItem: IXmlSerializable
   {
      private object _objectValue;
      private EndpointAddress _eprValue;

      public object ObjectValue
      {
         get { return _objectValue; }
      }

      public EndpointAddress EprValue
      {
         get { return _eprValue; }
      }

      public EnumerationItem()
      {         
      }

      public EnumerationItem(EndpointAddress epr, object value)
      {
         _eprValue = epr;
         _objectValue = value;
      }

      public EnumerationItem(EndpointAddress epr)
      {
         _eprValue = epr;
      }

      public XmlSchema GetSchema()
      {
         return null;
      }

      public void ReadXml(XmlReader reader)
      {         
         if (EnumerationModeExtension.CurrentEnumerationMode == EnumerationMode.EnumerateObjectAndEPR)
         {
            reader.ReadStartElement("Item", Management.Const.Namespace);
            XmlSerializer serializer = new XmlSerializer(EnumerationModeExtension.CurrentEnumeratedType);
            _objectValue = serializer.Deserialize(reader);            
         }
         _eprValue = EndpointAddress.ReadFrom(AddressingVersionExtension.CurrentVersion, reader);         
         if (EnumerationModeExtension.CurrentEnumerationMode == EnumerationMode.EnumerateObjectAndEPR)
         {
            reader.ReadEndElement();
         }
      }

      public void WriteXml(XmlWriter writer)
      {
         if (EnumerationModeExtension.CurrentEnumerationMode == EnumerationMode.EnumerateObjectAndEPR)
         {
            writer.WriteStartElement("Item", Management.Const.Namespace);
            XmlSerializer serializer = new XmlSerializer(_objectValue.GetType());
            serializer.Serialize(writer, _objectValue);
         }
         _eprValue.WriteTo(AddressingVersionExtension.CurrentVersion, writer);
         if (EnumerationModeExtension.CurrentEnumerationMode == EnumerationMode.EnumerateObjectAndEPR)
         {
            writer.WriteEndElement();
         }
      }
   }
}