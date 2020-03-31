using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pak.ApplicationServices;

namespace OrderApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly OrderApplicationService appService;

        public OrderController(ILogger<OrderController> logger, OrderApplicationService appService)
        {
            this.logger = logger;
            this.appService = appService;
        }

        [HttpPost("create")]
        public Guid When([FromBody]CreateOrder cmd)
        {
            var orderId = appService.When(cmd);
            return orderId;
        }

        [HttpPost("addPackage")]
        public Guid When([FromBody]AddPackage cmd)
        {
            var packageId = appService.When(cmd);
            return packageId;
        }

        [HttpPost("removePackage")]
        public void When([FromBody]RemovePackage cmd)
        {
            appService.When(cmd);
        }

        [HttpPost("submit")]
        public void When([FromBody]SubmitOrder cmd)
        {
            appService.When(cmd);
        }

        [HttpPost("cancel")]
        public void When([FromBody]CancelOrder cmd)
        {
            appService.When(cmd);
        }
    }
}
