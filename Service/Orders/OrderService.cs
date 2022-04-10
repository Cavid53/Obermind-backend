using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Service.Common;
using Service.Common.Exceptions;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Orders
{
    public interface IOrderService : ICrudService<OrderResource>
    {
        Task DeleteOrder(int id);
        Task OrderSubmit(int id);
    }

    public class OrderService : CrudService<OrderResource, Order>, IOrderService
    {
        private IAppDbContext _dbContext;
        private IMapper _mapper;

        public OrderService(IAppDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        //delete order with order details
        public async Task DeleteOrder(int id)
        {
            var order = DbContext.GetDbSet<Order>().Find(id);

            if (order == null) throw new NotFoundException(nameof(Order), id);

            var orderDetails = DbContext.GetDbSet<OrderItem>().Where(e => e.OrderId == order.Id);

            if (orderDetails.Any())
            {
                foreach (var item in orderDetails)
                {
                    item.SoftDeleted = true;
                }
            }

            order.SoftDeleted = true;

            await DbContext.SaveChangesAsync();
        }

        public async Task OrderSubmit(int id)
        {
            var order = DbContext.GetDbSet<Order>().Find(id);

            if (order == null) throw new NotFoundException(nameof(Order), id);

            order.Status = OrderStatus.Submitted;

            await DbContext.SaveChangesAsync();
        }
    }
}
