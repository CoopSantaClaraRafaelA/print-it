using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrintIt.Core;
using PrintIt.Core.Decorators;
using PrintIt.Core.Utils.Logger;

namespace PrintIt.ServiceHost.Controllers
{
    [ApiController]
    [Route("print")]
    public class PrintController : ControllerBase
    {
        private readonly IPdfPrintService _pdfPrintService;
        private readonly IPrinterService  _printerService;

        public PrintController(IPdfPrintService pdfPrintService, IPrinterService printerService)
        {
            _pdfPrintService = pdfPrintService;
            _printerService = printerService;
        }

        [HttpPost]
        [Route("from-pdf")]
        public async Task<IActionResult> PrintFromPdf([FromForm] PrintFromTemplateRequest request)
        {
            try
            {
                var logger = new FileLoggerCustom(request.PrinterPath, Guid.NewGuid());

                var printerService = new LoggerPrinterServiceDecorator(_printerService, logger);

                logger.Information($"Received print request for PDF file: {request.PdfFile.FileName} on printer: {request.PrinterPath}");


                await using Stream pdfStream = request.PdfFile.OpenReadStream();

                if (!printerService.GetInstalledPrinters().Any(printer => printer.Equals(request.PrinterPath, StringComparison.OrdinalIgnoreCase)))
                {
                    printerService.InstallPrinter(request.PrinterPath);
                }

                var printService = new LoggerPdfPrintServiceDecorator(_pdfPrintService, logger);

                printService.Print(pdfStream,
                    printerName: request.PrinterPath,
                    pageRange: request.PageRange,
                    numberOfCopies: request.Copies ?? 1,
                    request.PdfFile.FileName);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    public sealed class PrintFromTemplateRequest
    {
        [Required]
        public IFormFile PdfFile { get; set; }

        [Required]
        public string PrinterPath { get; set; }

        public string PageRange { get; set; }

        public int? Copies { get; set; }
    }
}
