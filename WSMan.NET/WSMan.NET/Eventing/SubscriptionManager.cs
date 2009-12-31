using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;
using WSMan.NET.Management;

namespace WSMan.NET.Eventing
{
   public class SubscriptionManager : EndpointReference
   {
      public SubscriptionManager(string identifier, Uri toUri, string deliveryResourceUri)
      {
         var subscriptionManagerAddress = new EndpointAddressBuilder
         {
            Uri = toUri
         };
         subscriptionManagerAddress.Headers.Add(new IdentifierHeader(identifier));
         subscriptionManagerAddress.Headers.Add(new ResourceUriHeader(deliveryResourceUri));
         _address = subscriptionManagerAddress.ToEndpointAddress();
      }

      public string Identifier
      {
         get
         {
            return IdentifierHeader.GetFrom(_address.Headers).Value;
         }
      }

      public string ResourceUri
      {
         get
         {
            return ResourceUriHeader.GetFrom(_address.Headers).ResourceUri;
         }
      }

      public SubscriptionManager()
      {
         
      }
   } 
}