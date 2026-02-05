using System;
using System.Collections.Generic;
using System.Text;

namespace Exceptions
{
    public class EmailNotMatchException : Exception
    {
        public string Message { get; set; }
        public EmailNotMatchException(string message)
        {
            Message = message;
        }
    }
}
