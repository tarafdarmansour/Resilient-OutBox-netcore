using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AccountingService.Models;
using AccountingService.Queries;
using MediatR;

namespace AccountingService.Controllers
{
    public class AccountingController : Controller
    {
        private readonly ILogger<AccountingController> _logger;
        private readonly IMediator _bus;

        public AccountingController(ILogger<AccountingController> logger, IMediator bus)
        {
            _logger = logger;
            _bus = bus;
        }

        public async Task<IActionResult> Index()
        {
            var cmd = new GetAccountingCustomerListQuery();
            var result = await _bus.Send(cmd);

            return View(result);
        }

    }
}
