using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderManagement.Application.Common.Logging
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName= typeof(TRequest).Name;
            var correlationId = Guid.NewGuid();

            _logger.LogInformation("İstek adı: {RequestName} işleniyor  Kimlik: {CorrelationId}", requestName, correlationId);
            var stopwatch = Stopwatch.StartNew();

            try
            {
                var response = next();
                stopwatch.Stop();

                _logger.LogInformation("İstek adı: {RequestName} işlendi  Kimlik: {CorrelationId} İşlem süresi: {ElapsedMilliseconds} ms", requestName, correlationId, stopwatch.ElapsedMilliseconds);

                return response;
            }
            catch (Exception)
            {
                stopwatch.Stop();

                _logger.LogError("İstek adı: {RequestName} işlenirken hata oluştu  Kimlik: {CorrelationId} İşlem süresi: {ElapsedMilliseconds} ms", requestName, correlationId, stopwatch.ElapsedMilliseconds);
                throw;
            }
        }
    }
}
