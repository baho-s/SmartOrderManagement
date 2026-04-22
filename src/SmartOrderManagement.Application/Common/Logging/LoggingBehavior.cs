using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Threading.Tasks;

namespace SmartOrderManagement.Application.Common.Logging
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName= typeof(TRequest).Name;
            var correlationId = Guid.NewGuid();

            // IP Adresini alıyoruz
            var ipAddress = GetIpAddress();

            _logger.LogInformation("İstek adı: {RequestName} işleniyor  Kimlik: {CorrelationId} IP:{IP}", requestName, correlationId, ipAddress);
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

        private string GetIpAddress()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null) return "Sistem";

            // "Proxy varmış gibi" kontrol et, yoksa normal IP'yi al.
            // Bu mantık seni gelecekteki deployment senaryolarına hazırlar.
            string ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();

            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Connection.RemoteIpAddress?.ToString();
            }

            return ip ?? "Bilinmiyor";
        }
    }
}
