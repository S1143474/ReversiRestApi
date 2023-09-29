using System;
using System.Collections.Generic;
using System.Text;

namespace Reversi.API.Domain.Common
{
    public abstract class BaseAuditableEntity
    {
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid UpdatedBy { get; set; }
        public int AmountOfUpdates { get; set; }
    }
}
