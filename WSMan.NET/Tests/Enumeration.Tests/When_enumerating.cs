using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using NUnit.Framework;
using Rhino.Mocks;
using WSMan.NET.Management;

namespace WSMan.NET.Enumeration.Tests
{
   [TestFixture]
   public class When_enumerating
   {
      private static readonly Uri ResourceUri = new Uri("http://tempuri.org");

      [Test]
      public void If_optimized_some_items_are_returned()
      {
         IEnumerationRequestHandler handler = MockRepository.GenerateMock<IEnumerationRequestHandler>();

         handler.Expect(x => x.Enumerate(null)).IgnoreArguments().Return(new [] {new EndpointAddress(ResourceUri.ToString())});

         EnumerationServer server = new EnumerationServer();
         server.Bind(ResourceUri, FilterMap.DefaultDialect, typeof (void), handler);

         OperationContextProxy.Current.AddHeader(new ResourceUriHeader(ResourceUri.ToString()));
         var response = server.Enumerate(new EnumerateRequest
                                           {
                                              EnumerationMode = EnumerationMode.EnumerateEPR,
                                              OptimizeEnumeration = OptimizeEnumeration.True
                                           });
         Assert.AreEqual(1, response.Items.Items.Count());
      }

      [Test]
      public void If_optimized_no_more_than_maximum_count_items_are_returned()
      {
         IEnumerationRequestHandler handler = MockRepository.GenerateMock<IEnumerationRequestHandler>();

         handler.Expect(x => x.Enumerate(null)).IgnoreArguments().Return(new[]
                {
                   new EndpointAddress(ResourceUri.ToString()),
                   new EndpointAddress(ResourceUri.ToString()),
                   new EndpointAddress(ResourceUri.ToString())
                });

         EnumerationServer server = new EnumerationServer();
         server.Bind(ResourceUri, FilterMap.DefaultDialect, typeof(void), handler);

         OperationContextProxy.Current.AddHeader(new ResourceUriHeader(ResourceUri.ToString()));
         var response = server.Enumerate(new EnumerateRequest
         {
            EnumerationMode = EnumerationMode.EnumerateEPR,
            OptimizeEnumeration = OptimizeEnumeration.True,
            MaxElements = new MaxElements(2)
         });
         Assert.AreEqual(2, response.Items.Items.Count());
      }
   }
   
}