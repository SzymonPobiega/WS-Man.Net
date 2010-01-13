using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using WSMan.NET.Enumeration;

namespace WSMan.NET.Eventing.Tests
{
   [TestFixture]
   public class When_pulling
   {      
      private static readonly Uri ResourceUri = new Uri("http://tempuri.org");

      [Test(Description = "R7.2.13-5")]
      [ExpectedException(typeof(FaultException<FaultException>))]
      public void If_pull_returns_no_items_exception_is_thrown()
      {
         IEventingRequestHandler<int> mockHandler = MockRepository.GenerateMock<IEventingRequestHandler<int>>();
         OperationContextProxy.Dummy.LocalAddress = ResourceUri;

         EventingServer server = new EventingServer();
         server.BindWithPullDelivery(ResourceUri, FilterMap.DefaultDialect, typeof(void), mockHandler, ResourceUri);
         SubscribeResponse subscribeResponse = server.Subscribe(ResourceUri.ToString(), null, new SubscribeRequest
             {
                Delivery = Delivery.Pull(),
                Expires = Expires.FromTimeSpan(TimeSpan.Zero)
             });
         server.Pull(new PullRequest
            {
               EnumerationContext = subscribeResponse.EnumerationContext,
            });
      }

      [Test]
      [ExpectedException(typeof(FaultException))]
      public void If_invalid_subscription_context_specified_exception_is_thrown()
      {
         OperationContextProxy.Dummy.LocalAddress = ResourceUri;

         EventingServer server = new EventingServer();         
         server.Pull(new PullRequest
         {
            EnumerationContext = new EnumerationContextKey("aaa"),
         });
      }      
   }
}
