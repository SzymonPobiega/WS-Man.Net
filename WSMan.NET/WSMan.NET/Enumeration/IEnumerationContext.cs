namespace WSMan.NET.Enumeration
{
    public interface IEnumerationContext
    {
        string Context { get; }
        object Filter { get; }
    }
}