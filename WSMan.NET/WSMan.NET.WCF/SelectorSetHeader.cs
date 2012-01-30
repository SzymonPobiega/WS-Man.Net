﻿using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml;

namespace WSMan.NET.WCF
{
   public sealed class SelectorSetHeader : AddressHeader
   {
      internal const string ElementName = "SelectorSet";
      private readonly List<Selector> _selectors;

      public override string Name
      {
         get { return ElementName; }
      }
      public override string Namespace
      {
         get { return Const.ManagementNamespace; }
      }

      public List<Selector> Selectors
      {
         get { return _selectors; }
      }

      public SelectorSetHeader(params Selector[] selectors)
      {
         _selectors = new List<Selector>(selectors);
      }

      public SelectorSetHeader(IEnumerable<Selector> selectors)
      {
         _selectors = new List<Selector>(selectors);
      }

      private SelectorSetHeader()
      {
         _selectors = new List<Selector>();
      }

      public static IEnumerable<Selector> GetCurrent()
      {
         SelectorSetHeader header = OperationContextProxy.Current.FindHeader<SelectorSetHeader>();
         if (header == null)
         {
            return new Selector[] {};
         }
         return header.Selectors;
      }

      public static SelectorSetHeader ReadFrom(Message message)
      {
         return ReadFrom(message.Headers);
      }

      public static SelectorSetHeader ReadFrom(EndpointAddress address)
      {
          AddressHeader header = address.Headers.FindHeader(ElementName, Const.ManagementNamespace);
         if (header == null)
         {
            return null;
         }
         using (XmlDictionaryReader readerAtHeader = header.GetAddressHeaderReader())
         {
            return ReadFrom(readerAtHeader);
         }
      }
      
      public static SelectorSetHeader ReadFrom(XmlReader reader)
      {
         SelectorSetHeader result = new SelectorSetHeader();
         reader.ReadStartElement(ElementName, Const.ManagementNamespace);
         while (reader.LocalName == Selector.ElementName)
         {
            Selector newSelector = Selector.ReadFrom(reader);
            result.Selectors.Add(newSelector);
         }
         if (reader.NodeType == XmlNodeType.EndElement)
         {
            reader.ReadEndElement();
         }
         return result;
      }      
      
      public static SelectorSetHeader ReadFrom(MessageHeaders messageHeaders)
      {
         SelectorSetHeader result;
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
      
      protected override void OnWriteAddressHeaderContents(XmlDictionaryWriter writer)
      {
         foreach (Selector selector in Selectors)
         {
            selector.WriteTo(writer);
         }
      }
   }
}