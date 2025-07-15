using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PrintIt.Core.Decorators
{
    public class PdfPrintServiceDecorator : IPdfPrintService
    {
        private readonly IPdfPrintService _innerService;

        public PdfPrintServiceDecorator(IPdfPrintService innerService)
        {
            _innerService = innerService ?? throw new ArgumentNullException(nameof(innerService));
        }

        public virtual void Print(Stream pdfStream, string printerName, string pageRange = null, int numberOfCopies = 1, string documentName = "document")
        {
            // Add any additional logic here if needed
            _innerService.Print(pdfStream, printerName, pageRange, numberOfCopies, documentName);
        }
    }
}
