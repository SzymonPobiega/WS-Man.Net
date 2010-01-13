using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using NUnit.Framework;

namespace WSMan.NET.Enumeration.Tests
{
   [TestFixture]
   public class When_pulling
   {      
      [Test]
      [ExpectedException(typeof(FaultException))]
      public void If_invalid_subscription_context_specified_exception_is_thrown()
      {
         EnumerationServer server = new EnumerationServer();
         PullRequest request = new PullRequest
                                  {
                                     EnumerationContext = new EnumerationContextKey("aaa")
                                  };
         server.Pull(request);
      }      
   }
}