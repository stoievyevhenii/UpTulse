using System;
using System.Collections.Generic;
using System.Text;

namespace UpTulse.Core.Exceptions
{
    public class DbRecordNotFoundException : Exception
    {
        public DbRecordNotFoundException(string message) : base(message)
        {
        }

        public DbRecordNotFoundException(Type type) : base($"{type} is missing")
        {
        }
    }
}