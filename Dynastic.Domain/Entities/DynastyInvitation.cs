namespace Dynastic.Domain.Entities;

public class DynastyInvitation : Base
{
    public bool IsRedeemed { get; set; }
    public Guid DynastyId { get; set; }
}