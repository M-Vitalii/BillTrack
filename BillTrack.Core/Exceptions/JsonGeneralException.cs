namespace BillTrack.Core.Exceptions;

public class JsonGeneralException : Exception
{
    public JsonGeneralException()
    {
    }

    public JsonGeneralException(string message) : base(message)
    {
    }

    public JsonGeneralException(string message, Exception innerException) : base(message, innerException)
    {
    }
}