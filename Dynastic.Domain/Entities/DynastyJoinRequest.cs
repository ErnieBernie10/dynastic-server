namespace Dynastic.Domain.Entities;

public class DynastyJoinRequest : Base
{
    public bool IsApproved { get; set; }
    public bool IsRedeemed { get; set; }
    public Guid DynastyId { get; set; }
    public string UserId { get; set; }
}