namespace IMS.Business.Handlers.Auth;

[Serializable]
public class TokenInvalidException : Exception
{
    public TokenInvalidException()
    {
    }

    public TokenInvalidException(string? message) : base(message)
    {
    }

    public TokenInvalidException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}