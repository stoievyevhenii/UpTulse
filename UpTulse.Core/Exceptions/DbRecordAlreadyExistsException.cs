using System;
using System.Collections.Generic;
using System.Text;

namespace UpTulse.Core.Exceptions
{
    public class DbRecordAlreadyExistsException(string message) : Exception(message)
    {
    }
}