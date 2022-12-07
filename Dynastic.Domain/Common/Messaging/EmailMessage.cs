namespace Dynastic.Domain.Common.Messaging;

public class EmailMessage
{
    public string To { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }
}