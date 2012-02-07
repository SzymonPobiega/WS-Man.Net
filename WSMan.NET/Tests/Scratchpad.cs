using System;
using System.Dynamic;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace WSMan.NET
{
    [TestFixture]
    public class Scratchpad
    {
        [Test]
        public void Dynamic_can_implement_any_interface()
        {
            dynamic dynamicExpando = new CastableDynamicObject();
            var b = (IDisposable)dynamicExpando;
            b.Dispose();
        }

        public class CastableDynamicObject : System.Dynamic.DynamicObject
        {

            public override bool TryConvert(ConvertBinder binder, out object result)
            {
                return base.TryConvert(binder, out result);
            }
        }
    }
}
// ReSharper restore InconsistentNaming
