namespace Dynastic.Domain.Common.Messaging;

public class InviteToDynastyMessage
{
    public string To { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }
}