using System;

namespace PrintIt.Core.Decorators
{
    public class PrinterServiceDecorator : IPrinterService
    {
        private readonly IPrinterService _innerService;

        public PrinterServiceDecorator(IPrinterService innerService)
        {
            _innerService = innerService ?? throw new ArgumentNullException(nameof(innerService));
        }

        public virtual string[] GetInstalledPrinters()
        {
            return _innerService.GetInstalledPrinters();
        }

        public virtual void InstallPrinter(string printerPath)
        {
            _innerService.InstallPrinter(printerPath);
        }

        public virtual IPrinterService GetInnerService()
        {
            return _innerService;
        }
    }
}
