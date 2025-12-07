using System;
using System.Collections.Generic;
using System.Text;

namespace UpTulse.DataAccess
{
    public class DatabaseConfiguration
    {
        public string ConnectionString { get; set; }
        public bool UseInMemoryDatabase { get; set; }
    }
}