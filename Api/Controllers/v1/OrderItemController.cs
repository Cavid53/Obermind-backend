using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Common;
using Service.OrderItems;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize]
    public class OrderItemController : BaseController
    {
        private readonly IOrderItemService  _service;

        public OrderItemController(IOrderItemService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("GetAllByOrderId/{id}")]
        public async Task<IEnumerable<OrderItemResource>> GetAllByOrderId([FromRoute][Required] int id)
        {
            return await _service.GetAllByOrderId(id);
        }

        [HttpPost]
        [Route("upsert")]
        public async Task<UpsertReplyResource> Upsert([FromBody][Required] OrderItemResource resource)
        {
            return await _service.UpsertOrderItem(resource);
        }

        [HttpGet]
        [Route("{id}")]
        public OrderItemResource GetById([FromRoute][Required] int id)
        {
            return _service.GetById(id);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task Delete([FromRoute][Required] int id)
        {
            await _service.DeleteOrderItem(id);
        }
    }
}
