using System;

namespace Domain.Common
{
    public abstract class AuditingEntity : BaseEntity
    {
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedAt { get; set; }
    }
}
