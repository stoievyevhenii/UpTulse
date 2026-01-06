using System;
using System.Collections.Generic;
using System.Text;

namespace UpTulse.Core.Exceptions
{
    public class ConfigurationNotFoundException(string message) : Exception(message)
    {
    }
}