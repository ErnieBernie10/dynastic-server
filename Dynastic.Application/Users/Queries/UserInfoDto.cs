namespace Dynastic.Application.Users.Queries;

public class UserInfoDto
{
    public string UserId { get; set; }
    public string? Firstname { get; set; }
    public string MiddleName { get; set; }
    public string Lastname { get; set; }
    public DateTime BirthDate { get; set; }
}
