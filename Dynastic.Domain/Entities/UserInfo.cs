namespace Dynastic.Domain.Entities;

public class UserInfo : Base
{
    public string UserId { get; set; }
    public string? Firstname { get; set; }
    public string MiddleName { get; set; }
    public string Lastname { get; set; }
    public DateTime BirthDate { get; set; }
}