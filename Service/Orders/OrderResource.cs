using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using Service.Common;
using Service.Mappings;
using System;

namespace Service.Orders
{
    public class OrderResource : IBaseResource, IResourceMapper
    {
        public int? Id { get; set; }
        public string No { get; set; }
        public OrderStatus Status { get; set; }
        public float TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public string CreatedBy { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Order, OrderResource>().ReverseMap();
        }
    }

    public class OrderResourceValidator : AbstractValidator<OrderResource>
    {
        public OrderResourceValidator()
        {
            RuleFor(e => e.No).NotEmpty().MaximumLength(100);
        }
    }
}
