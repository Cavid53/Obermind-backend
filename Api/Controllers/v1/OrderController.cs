using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Common;
using Service.Orders;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize]
    public class OrderController : BaseController
    {
        private readonly IOrderService _service;

        public OrderController(IOrderService service)
        {
            _service = service;
        }


        [HttpGet]
        [Route("all")]
        public async Task<IEnumerable<OrderResource>> GetAll()
        {
            return await _service.GetAll();
        }

        [HttpGet]
        [Route("{id}")]
        public OrderResource GetById([FromRoute][Required] int id)
        {
            return _service.GetById(id);
        }

        [HttpPost]
        [Route("upsert")]
        public async Task<UpsertReplyResource> Upsert([FromBody][Required] OrderResource resource)
        {
            return await _service.Upsert(resource);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task Delete([FromRoute][Required] int id)
        {
            await _service.DeleteOrder(id);
        }

        [HttpGet]
        [Route("Submit/{id}")]
        public async Task OrderSubmit([FromRoute][Required] int id)
        {
            await _service.OrderSubmit(id);
        }

    }
}
