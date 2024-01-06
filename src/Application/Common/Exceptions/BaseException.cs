using System.Net;

namespace Blog.Application.Common.Exceptions;

public abstract class BaseException : Exception
{
    public int StatusCode { get; set; }
    public string? Type { get; set; }

    protected BaseException():base("Có lỗi xảy ra")
    {
        this.StatusCode = (int)HttpStatusCode.InternalServerError;
        
    }

    protected BaseException(string? message) : base(message)
    {
        this.StatusCode = (int)HttpStatusCode.InternalServerError;
        
    }
    protected BaseException(string? message, int statusCode) : base(message)
    {
        this.StatusCode = statusCode;
    }
}
