using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Services.Messages;

namespace Ocas.Domestic.Apply.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{orderId}")]
        public async Task<ActionResult<Order>> GetOrder(Guid orderId)
        {
            return Ok(await _mediator.Send(new GetOrder
            {
                OrderId = orderId,
                User = User
            }));
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder([FromBody]CreateOrderInfo createOrderInfo)
        {
            return Ok(await _mediator.Send(new CreateOrder
            {
                ApplicationId = createOrderInfo.ApplicationId,
                User = User
            }));
        }

        [HttpPost("{orderId}/[action]")]
        public async Task<ActionResult<FinancialTransaction>> Pay(Guid orderId, [FromBody]PayOrderInfo payOrderInfo)
        {
            return Ok(await _mediator.Send(new PayOrder
            {
                OrderId = orderId,
                User = User,
                PayOrderInfo = payOrderInfo,
                IsOfflinePayment = false
            }));
        }
    }
}
