using Domain.Common;

namespace Domain.Entities
{
    public class OrderItem : AuditingEntity
    {
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public float ProductPrice { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
