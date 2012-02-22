namespace WSMan.NET.Eventing.Server
{
    public interface IEventSink
    {
        void Push(object evnt);
    }
}