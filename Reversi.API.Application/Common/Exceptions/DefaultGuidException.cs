using System;
using System.Collections.Generic;
using System.Text;

namespace Reversi.API.Application.Common.Exceptions
{
    public class DefaultGuidException : Exception
    {
        public DefaultGuidException(string message)
            : base(message)
        {
        }
    }
}
