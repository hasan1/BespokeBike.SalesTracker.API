namespace BespokeBike.SalesTracker.API.Extensions
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public int StatusCode { get; set; }
        public Guid TraceId { get; set; }

        public ApiResponse(bool success, string message, T data, int statusCode, Guid traceId)
        {
            Success = success;
            Message = message;
            Data = data;
            StatusCode = statusCode;
            TraceId = traceId;
        }
    }
}
