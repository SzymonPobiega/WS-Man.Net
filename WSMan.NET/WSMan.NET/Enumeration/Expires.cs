using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace WSMan.NET.Enumeration
{
   public class Expires : IXmlSerializable
   {
      private const XmlDateTimeSerializationMode DateSerializationFormat = XmlDateTimeSerializationMode.Local;

      private object _value;

      public object Value
      {
         get { return _value; }
      }

      public Expires()
      {         
      }

      private Expires(object value)
      {
         _value = value;
      }

      public static Expires FromDateTime(DateTime value)
      {
         return new Expires(value);
      }

      public static Expires FromTimeSpan(TimeSpan value)
      {
         return new Expires(value);
      }

      public XmlSchema GetSchema()
      {
         return null;
      }

      public void ReadXml(XmlReader reader)
      {
         string valueTmp = reader.ReadString();
         try
         {
            _value = XmlConvert.ToTimeSpan(valueTmp);
         }
         catch (Exception)
         {            
            _value = XmlConvert.ToDateTime(valueTmp, DateSerializationFormat);            
         }
         reader.ReadEndElement();
      }

      public void WriteXml(XmlWriter writer)
      {
         if (_value is DateTime)
         {
            writer.WriteValue(XmlConvert.ToString((DateTime)_value, DateSerializationFormat));  
         }
         else
         {            
            writer.WriteValue(XmlConvert.ToString((TimeSpan)_value));  
         }         
      }
   }
}