namespace BillTrack.Domain.Entities;

public class User : AuditableEntity
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}