using System;
using System.Linq;
using System.Collections.Generic;
using WSMan.NET.Addressing;
using WSMan.NET.Management;
using WSMan.NET.Transfer;

namespace ManagementWebDemo
{
    public class Handler : IManagementRequestHandler
    {
        public bool CanHandle(string resourceUri)
        {
            return true;
        }

        public object HandleGet(string fragmentExpression, IEnumerable<Selector> selectors)
        {
            var selector = selectors.FirstOrDefault() ?? new Selector("-","-");

            Console.WriteLine("Server: requesting fragment {0} with selector [{1}/{2}]", fragmentExpression, selector.Name, selector.Value);
            Console.WriteLine();
            return new XmlFragment<SampleData>(new SampleData());
        }

        public object HandlePut(string fragmentExpression, IEnumerable<Selector> selectors, ExtractBodyDelegate extractBodyCallback)
        {
            var value = (SampleData)extractBodyCallback(typeof(SampleData));
            Console.WriteLine("Server: putting fragment {0}: {1}", fragmentExpression, value);
            Console.WriteLine();
            return value;
        }

        public EndpointReference HandleCreate(ExtractBodyDelegate extractBodyCallback)
        {
            return new EndpointReference("http://tempuri.org");
        }

        public void HandlerDelete(IEnumerable<Selector> selectors)
        {
            Console.WriteLine("Server: deleting resource with selector [{0}/{1}]", selectors.First().Name, selectors.First().AddressReference);
            Console.WriteLine();
        }
    }
}