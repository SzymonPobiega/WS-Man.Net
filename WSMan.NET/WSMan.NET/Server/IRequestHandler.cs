using WSMan.NET.SOAP;

namespace WSMan.NET.Server
{
    public interface IRequestHandler
    {
        OutgoingMessage Handle(IncomingMessage request);
    }
}