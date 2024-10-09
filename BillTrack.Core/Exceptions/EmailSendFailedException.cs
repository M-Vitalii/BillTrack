namespace BillTrack.Core.Exceptions;

public class EmailSendFailedException : Exception
{
    public EmailSendFailedException(string message) : base(message) { }

    public EmailSendFailedException(string message, Exception innerException) 
        : base(message, innerException) { }
}
