using Microsoft.AspNetCore.Mvc;
using System;

namespace BespokeBike.SalesTracker.API.Extensions
{
    public static class ApiResponseExtensions
    {
        public static Guid GetCorrelationId(ControllerBase controller)
        {
            if (controller.Request.Headers.TryGetValue("correlationId", out var correlationIdHeader))
            {
                if (Guid.TryParse(correlationIdHeader, out var correlationId))
                {
                    return correlationId;
                }
            }
            return Guid.NewGuid();
        }

        public static ActionResult<ApiResponse<T>> ToApiResponse<T>(this ControllerBase controller, T data, string message, int statusCode)
        {
            var traceId = GetCorrelationId(controller);
            var response = new ApiResponse<T>(true, message, data, statusCode, Guid.NewGuid());
            return controller.StatusCode(statusCode, response);
        }

        public static ActionResult<ApiResponse<T>> ToApiResponse<T>(this ControllerBase controller, string message, int statusCode)
        {
            var traceId = GetCorrelationId(controller);
            var response = new ApiResponse<T>(false, message, default, statusCode, Guid.NewGuid());
            return controller.StatusCode(statusCode, response);
        }
    }
}
