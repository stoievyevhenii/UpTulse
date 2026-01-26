using System;
using System.Collections.Generic;
using System.Text;

namespace ItTechnologies.CoreUI.Exceptions
{
    public class ComponentNullException(string message) : Exception(message)
    {
    }
}