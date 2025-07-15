using System;
using System.IO;
using Serilog;
using Serilog.Events;

namespace PrintIt.Core.Utils.Logger
{
    public class FileLoggerCustom : ILogger
    {
        private readonly Guid _guid;
        private readonly ILogger _logger;

        public FileLoggerCustom(string logName, Guid guid)
        {
            _guid = guid;

            _logger = GetLogger(logName);
        }

        public void Write(LogEvent logEvent)
        {
            _logger.Write(logEvent);
        }

        public void Information(string messageTemplate)
        {
            _logger.Information($"{_guid} - {messageTemplate}");
        }

        private static ILogger GetLogger(string logName)
        {
            var logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            Directory.CreateDirectory(logDir);

            string fileName = logName.Trim().Replace("\\", string.Empty);

            var logFileName = Path.Combine(logDir, $"{fileName}", $"{DateTime.Now.ToString("dd-MM-yyyy")}.txt");

            return new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.File(logFileName, shared: true)
                .CreateLogger();
        }

        public void Error(string message)
        {
            _logger.Error(message);
        }
    }
}
