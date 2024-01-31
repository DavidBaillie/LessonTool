using System.Net;

namespace LessonTool.API.Infrastructure.Exceptions;

public class CosmosActionException : Exception
{
    public HttpStatusCode StatusCode { get; set; }

    public CosmosActionException() { }
    public CosmosActionException(string message, Exception inner = null) : base(message, inner) { }
    public CosmosActionException(string message, HttpStatusCode responseCode, Exception inner = null) : base(message, inner) 
    {
        StatusCode = responseCode;
    }
}
