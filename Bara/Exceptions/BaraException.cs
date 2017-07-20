using System;
using System.Collections.Generic;
using System.Text;

namespace Bara.Exceptions
{
    public class BaraException : Exception
    {
        public BaraException() : base("Bara throw an exception~") { }

        public BaraException(Exception ex) : base("Bara throw an exception~", ex) { }

        public BaraException(string message) : base(message) { }

        public BaraException(string message, Exception ex) : base(message, ex) { }
    }
}
