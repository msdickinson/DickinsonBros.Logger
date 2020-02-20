using DickinsonBros.Logger;
using DickinsonBros.Logger.Abstractions;
using DickinsonBros.Redactor.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DickinsonBros.Logger
{
    public class LoggingService<T> : ILoggingService<T>
    {
        internal readonly ILogger<T> _logger;
        internal readonly IRedactorService _redactorService;
        internal readonly ICorrelationService _correlationService;
        public LoggingService(ILogger<T> logger, IRedactorService redactorService, ICorrelationService correlationService)
        {
            _logger = logger;
            _redactorService = redactorService;
            _correlationService = correlationService;
        }

        public void LogDebugRedacted(string message, IDictionary<string, object> properties = null)
        {
            Log(LogLevel.Debug, message, properties, null);
        }

        public void LogInformationRedacted(string message, IDictionary<string, object> properties = null)
        {
            Log(LogLevel.Information, message, properties, null);
        }

        public void LogWarningRedacted(string message, IDictionary<string, object> properties = null)
        {
            Log(LogLevel.Warning, message, properties, null);
        }

        public void LogErrorRedacted(string message, Exception exception, IDictionary<string, object> properties = null)
        {
            Log(LogLevel.Error, message, properties, exception);
        }

        public void Log(LogLevel logLevel, string message, IDictionary<string, object> properties = null, Exception exception = null)
        {
            var propertiesRedacted = new List<KeyValuePair<string, object>>();

            if (properties != null)
            {
                propertiesRedacted.AddRange
                (
                    properties.Select
                    (
                        property => new KeyValuePair<string, object>(property.Key, _redactorService.Redact(property.Value))
                    ).ToList()
                );
            }

            if (_correlationService.CorrelationId != null)
            {
                propertiesRedacted.Add(new KeyValuePair<string, object>("CorrelationId", _correlationService.CorrelationId));
            }

            _logger.Log(logLevel, 1, ((object)(new LogState(propertiesRedacted))), exception, (_, exception) => Formatter(message, propertiesRedacted));
        }

        internal string Formatter(string message, IList<KeyValuePair<string, object>> propertiesRedacted)
        {
            return message +
                    Environment.NewLine +
                    String.Concat
                    (
                        propertiesRedacted.Select
                        (
                            keyValuePair => $"{keyValuePair.Key}: {keyValuePair.Value}" +
                                            Environment.NewLine
                        )
                    );
        }

    }


}
