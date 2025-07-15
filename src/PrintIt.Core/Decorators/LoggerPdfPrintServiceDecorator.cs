using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Text;
using PrintIt.Core.Utils;
using Serilog;

namespace PrintIt.Core.Decorators
{
    public class LoggerPdfPrintServiceDecorator : PdfPrintServiceDecorator
    {
        private readonly ILogger _logger;

        public LoggerPdfPrintServiceDecorator(IPdfPrintService innerService, ILogger logger)
            : base(innerService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override void Print(Stream pdfStream, string printerName, string pageRange = null, int numberOfCopies = 1, string documentName = "document")
        {
            _logger.Information($"Starting print job for {documentName} on printer {printerName} with {numberOfCopies} copies.");

            try
            {
                base.Print(pdfStream, printerName, pageRange, numberOfCopies, documentName);
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to print {documentName} on printer {printerName}.");
                _logger.Error($"Exception: {ex.Message}");
                throw;
            }

            _logger.Information($"Finished print job for {documentName} on printer {printerName}.");
        }
    }
}
