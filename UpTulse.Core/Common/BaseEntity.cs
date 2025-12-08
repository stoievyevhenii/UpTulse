using System;
using System.Collections.Generic;
using System.Text;

namespace UpTulse.Core.Common
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; } = Guid.CreateVersion7();
    }
}