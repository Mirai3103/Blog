using System.Net;

namespace Blog.Application.Common.Exceptions;

public class ConflictException : BaseException
{
    public ConflictException() : base(null, (int)HttpStatusCode.Conflict)
    {
    }

    public ConflictException(string message) : base(message, (int)HttpStatusCode.Conflict)
    {
    }
}
