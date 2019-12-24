using System;

namespace DickinsonBros.Logger.Abstractions
{
    public interface ICorrelationService
    {
        string CorrelationId { get; set; }
    }
}
