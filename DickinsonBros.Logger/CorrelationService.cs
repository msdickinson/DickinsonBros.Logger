using DickinsonBros.Logger.Abstractions;
using System.Threading;

namespace DickinsonBros.Logger
{
    public class CorrelationService : ICorrelationService
    {
        public string CorrelationId
        {
            get
            {
                return _asyncLocalCorrelationId.Value;
            }
            set
            {
                _asyncLocalCorrelationId.Value = value;
            }
        }
        internal AsyncLocal<string> _asyncLocalCorrelationId { get; set; }

        public CorrelationService()
        {
            _asyncLocalCorrelationId = new AsyncLocal<string>();
        }
    }
}
