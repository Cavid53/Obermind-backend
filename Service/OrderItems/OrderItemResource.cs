using AutoMapper;
using Domain.Entities;
using FluentValidation;
using Service.Common;
using Service.Mappings;

namespace Service.OrderItems
{
    public class OrderItemResource : IBaseResource, IResourceMapper
    {
        public int? Id { get; set; }
        public int OrderId { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public float ProductPrice { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<OrderItem, OrderItemResource>().ReverseMap();
        }

        public class OrderDetailResourceValidator : AbstractValidator<OrderItemResource>
        {
            public OrderDetailResourceValidator()
            {
                RuleFor(e => e.ProductName).NotEmpty().MaximumLength(150);
                RuleFor(e => e.ProductDescription).NotEmpty().MaximumLength(150);
                RuleFor(e => e.ProductPrice).NotEmpty();
                RuleFor(e => e.OrderId).InclusiveBetween(0, int.MaxValue);
            }
        }
    }
}
