using System;
using Serilog;

namespace PrintIt.Core.Decorators
{
    public class LoggerPrinterServiceDecorator : PrinterServiceDecorator
    {
        private readonly ILogger _logger;

        public LoggerPrinterServiceDecorator(IPrinterService innerService, ILogger logger)
            : base(innerService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override string[] GetInstalledPrinters()
        {
            _logger.Information("Retrieving installed printers.");
            string[] ret;

            try
            {
                ret = base.GetInstalledPrinters();
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to retrieve installed printers.");
                _logger.Error($"Exception: {ex.Message}");
                throw;
            }

            _logger.Information($"Found {ret.Length}.");

            return ret;
        }

        public override void InstallPrinter(string printerPath)
        {
            _logger.Information($"Installing printer at path: {printerPath}");

            try
            {
                base.InstallPrinter(printerPath);
            }
            catch (Exception ex )
            {
                _logger.Error($"Failed to install printer at path: {printerPath}");
                _logger.Error($"Exception: {ex.Message}");
                throw;
            }

            _logger.Information($"Printer installed successfully at path: {printerPath}");
        }

        public virtual ILogger GetLogger()
        {
            return _logger;
        }
    }
}
