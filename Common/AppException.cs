using System.Net;

namespace BankApi.Common;

public class AppException(string errorCode, HttpStatusCode statusCode = HttpStatusCode.BadRequest, params object[] args)
    : Exception($"Business error: {errorCode}")
{
    public string ErrorCode { get; } = errorCode;
    public object[] Args { get; } = args;
    public HttpStatusCode StatusCode { get; } = statusCode;
}