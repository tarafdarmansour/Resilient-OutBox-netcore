using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskService.Models;
using TaskService.Queries;

namespace TaskService.Controllers
{
    public class TaskController : Controller
    {
        private readonly ILogger<TaskController> _logger;
        private readonly IMediator _bus;

        public TaskController(ILogger<TaskController> logger, IMediator bus)
        {
            _logger = logger;
            _bus = bus;
        }

        public async Task<IActionResult> Index()
        {
            var cmd = new GetTaskCustomerListQuery();
            var result = await _bus.Send(cmd);

            return View(result);
        }

    }
}
