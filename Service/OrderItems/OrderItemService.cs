using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Service.Common;
using Service.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.OrderItems
{
    public interface IOrderItemService : ICrudService<OrderItemResource>
    {
        Task<IEnumerable<OrderItemResource>> GetAllByOrderId(int orderId);
        Task<UpsertReplyResource> UpsertOrderItem(OrderItemResource resource);
        Task DeleteOrderItem(int id);

    }

    public class OrderItemService : CrudService<OrderItemResource, OrderItem>, IOrderItemService
    {
        private IAppDbContext _dbContext;
        private IMapper _mapper;

        public OrderItemService(IAppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        //delete order item 
        public async Task DeleteOrderItem(int id)
        {
            var entity = DbContext.GetDbSet<OrderItem>().Find(id);

            if (entity == null) throw new NotFoundException(nameof(OrderItem), id);

            var order = DbContext.GetDbSet<Order>().FirstOrDefault(e => e.Id == entity.OrderId);

            if (order != null)
                order.TotalPrice -= entity.ProductPrice;

            entity.SoftDeleted = true;

            await DbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrderItemResource>> GetAllByOrderId(int orderId)
        {
            return await DbContext.GetDbSet<OrderItem>()
              .Where(e => e.OrderId == orderId)
              .ProjectTo<OrderItemResource>(Mapper.ConfigurationProvider)
              .ToListAsync();
        }

        //add item to order
        public async Task<UpsertReplyResource> UpsertOrderItem(OrderItemResource resource)
        {
            OrderItem entity;
            var order = _dbContext.GetDbSet<Order>().Where(e => e.Id == resource.OrderId).First();

            if (resource.Id.HasValue && resource.Id.Value > 0)
            {
                entity = DbContext.GetDbSet<OrderItem>().Find(resource.Id.Value);
                if (entity == null) throw new NotFoundException(nameof(OrderItem), resource.Id.Value);

                order.TotalPrice = (order.TotalPrice - entity.ProductPrice) + resource.ProductPrice;
            }
            else
            {
                entity = (OrderItem)Activator.CreateInstance(typeof(OrderItem));
                if (entity == null) throw new Exception($"{nameof(OrderItem)} couldn't be created");

                order.TotalPrice += resource.ProductPrice;

                DbContext.GetDbSet<OrderItem>().Add(entity);
            }

            Mapper.Map(resource, entity);

            await DbContext.SaveChangesAsync();

            return new UpsertReplyResource(entity.Id);

        }
    }
}
