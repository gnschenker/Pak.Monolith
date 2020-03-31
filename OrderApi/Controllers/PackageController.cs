using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pak.ApplicationServices;

namespace OrderApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PackageController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly PackageApplicationService appService;

        public PackageController(ILogger<PackageController> logger, PackageApplicationService appService)
        {
            this.logger = logger;
            this.appService = appService;
        }

        [HttpPost("setDeliveryWindow")]
        public void When([FromBody]SetDeliveryWindow cmd)
        {
            appService.When(cmd);
        }

        [HttpPost("printLabel")]
        public void When([FromBody]PrintLabel cmd)
        {
            appService.When(cmd);
        }
    }
}
