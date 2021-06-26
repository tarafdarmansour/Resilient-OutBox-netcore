using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CustomerService.Models;
using MediatR;
using CustomerService.Command;
using CustomerService.Queries;
using CustomerService.Messaging.RabbitMQ.Outbox;

namespace CustomerService.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly IMediator _bus;

        public CustomerController(ILogger<CustomerController> logger, IMediator bus)
        {
            _logger = logger;
            _bus = bus;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var cmd = new GetCustomerListQuery();
            var result = await _bus.Send(cmd);

            return View(result);
        }

        [HttpPost]
        public async Task<JsonResult> Delete([FromBody] DeleteCustomerCommand cmd)
        {
            var result = await _bus.Send(cmd);
            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<JsonResult> Update([FromBody] UpdateCustomerCommand cmd)
        {
            var result = await _bus.Send(cmd);
            return new JsonResult(result);
        }

        [HttpGet]
        public  ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromForm] CreateCustomerCommand cmd)
        {
            var result = await _bus.Send(cmd);
            return RedirectToAction("List");
        }
    }
}
