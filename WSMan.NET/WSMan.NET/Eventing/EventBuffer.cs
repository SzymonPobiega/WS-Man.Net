using System;
using System.Linq;
using System.Collections.Generic;

namespace WSMan.NET.Eventing
{
   public class EventBuffer : IEventingRequestHandlerContext
   {
      public const int DefaultSize = 100;

      public event EventHandler<EventArgs> ItemPushed;

      public bool IsEmpty
      {
         get { return _storage.Count == 0; }
      }
      
      public void Push(object @event)
      {         
         lock (_storage)
         {
            _storage.Enqueue(@event);            
            if (_storage.Count > _maxSize)
            {
               _storage.Dequeue();
            }
            OnItemPushed();
         }
      }
      public IEnumerable<object> FetchNotifications()
      {
         lock (_storage)
         {
            while (_storage.Count > 0)
            {
               yield return _storage.Dequeue();
            }
         }
      }

      private void OnItemPushed()
      {
         EventHandler<EventArgs> itemPushed = ItemPushed;
         if (itemPushed != null)
         {
            itemPushed(this, new EventArgs());
         }
      }

      public EventBuffer()
         : this(DefaultSize)
      {
      }
      public EventBuffer(int maxSize)
      {
         _maxSize = maxSize;
      }

      private readonly Queue<object> _storage = new Queue<object>();
      private readonly int _maxSize;      
   }
}