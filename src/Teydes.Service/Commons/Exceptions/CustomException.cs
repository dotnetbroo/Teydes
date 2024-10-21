namespace Teydes.Service.Commons.Exceptions;

public class CustomException : Exception
{
    public int statusCode { get; set; }

    public CustomException(int code, string message) : base(message)
    {
        statusCode = code;
    }

}
