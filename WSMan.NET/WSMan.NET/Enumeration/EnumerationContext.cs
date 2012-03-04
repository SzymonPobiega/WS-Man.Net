namespace WSMan.NET.Enumeration
{
   public class EnumerationContext : IEnumerationContext
   {
      private readonly string _context;
      private readonly object _filter;

      public EnumerationContext(string context, object filter)
      {
         _context = context;
         _filter = filter;
      }

      public string Context
      {
         get { return _context; }
      }

      public object Filter
      {
         get { return _filter; }
      }
   }
}