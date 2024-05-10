using System.Net;

namespace LessonTool.UI.Application.Exceptions;

public class BadHttpResponseException : Exception
{
    public HttpStatusCode StatusCode { get; set; }
    public HttpContent ResponseContent { get; set; } = null;

    public BadHttpResponseException(string message, HttpStatusCode statusCode) : base(message)
    {
        StatusCode = statusCode;
    }

    public BadHttpResponseException(HttpResponseMessage response) : base(response.ReasonPhrase)
    {
        StatusCode = response.StatusCode;
        ResponseContent = response.Content;
    }

    public BadHttpResponseException(string message, HttpResponseMessage response) : base(message)
    {
        StatusCode = response.StatusCode;
        ResponseContent = response.Content;
    }
}