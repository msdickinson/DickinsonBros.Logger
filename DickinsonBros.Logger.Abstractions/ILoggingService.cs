using System;
using System.Collections.Generic;
using System.Text;

namespace DickinsonBros.Logger.Abstractions
{
    public interface ILoggingService<out T>
    {
        void LogDebugRedacted(string message, IDictionary<string, object> properties = null);
        void LogInformationRedacted(string message, IDictionary<string, object> properties = null);
        void LogWarningRedacted(string messagee, IDictionary<string, object> properties = null);
        void LogErrorRedacted(string message, Exception exception, IDictionary<string, object> properties = null);
    }
}
