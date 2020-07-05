using System;
using System.Diagnostics;

namespace BuildLight.Common
{
    public abstract class UIControl
    {
        public UIControl()
        {
        }

        protected virtual void PresentError(Exception exception)
        {
            Console.WriteLine($"ERROR {exception}");
        }
    }
}
