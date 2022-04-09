using Domain.Common;
using Domain.Enums;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Order : AuditingEntity
    {
        public string No { get; set; }
        public float? TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public IList<OrderItem> OrderItems { get; set; }
        public OrderStatus Status { get; set; }
    }
}
