using System;
using Microsoft.AspNetCore.Mvc;
using PrintIt.Core;
using PrintIt.Core.Decorators;
using PrintIt.Core.Utils.Logger;
using Serilog;

namespace PrintIt.ServiceHost.Controllers
{
    [ApiController]
    [Route("printers")]
    public sealed class PrinterController : ControllerBase
    {
        private readonly IPrinterService _printerService;

        public PrinterController(IPrinterService printerService)
        {
            _printerService = printerService;
        }

        [HttpGet]
        [Route("list")]
        public IActionResult ListPrinters()
        {
            var printerService = new LoggerPrinterServiceDecorator(_printerService, new FileLoggerCustom("General", Guid.NewGuid()));

            string[] installedPrinters = printerService.GetInstalledPrinters();
            return Ok(installedPrinters);
        }
        
        [HttpPost]
        [Route("install")]
        public IActionResult InstallPrinter([FromQuery] string printerPath)
        {
            _printerService.InstallPrinter(printerPath);
            return Ok();
        }        
    }
}
