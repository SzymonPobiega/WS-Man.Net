using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace WSMan.NET.WCF
{
    public class AddressingVersionExtensionServiceBehaviorAttribute : Attribute, IServiceBehavior
    {
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher dispatcher in serviceHostBase.ChannelDispatchers)
            {
                foreach (EndpointDispatcher endpointDispatcher in dispatcher.Endpoints)
                {
                    ServiceEndpoint endpoint = serviceDescription.Endpoints.Find(endpointDispatcher.EndpointAddress.Uri);
                    var inspector = new MessageInspector(endpoint.Binding.MessageVersion.Addressing);
                    endpointDispatcher.DispatchRuntime.MessageInspectors.Add(inspector);
                }
            }
        }

        private class MessageInspector : IDispatchMessageInspector
        {
            public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
            {
                AddressingVersionExtension.Activate(_version);
                //ResourceUriHeader.ReadFrom(request);
                //MaxEnvelopeSizeHeader.ReadFrom(request);
                return null;
            }

            public void BeforeSendReply(ref Message reply, object correlationState)
            {                
            }

            public MessageInspector(AddressingVersion version)
            {
                _version = version;
            }

            private readonly AddressingVersion _version;
        }
    }
}