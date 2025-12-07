using System;
using System.Collections.Generic;
using System.Text;

namespace UpTulse.Core.Common
{
    public interface IAuditedEntity
    {
        public string CreatedBy { get; set; }
    }
}