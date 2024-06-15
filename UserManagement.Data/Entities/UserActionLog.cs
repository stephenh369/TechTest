using System;

namespace UserManagement.Models;

public class UserActionLog
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string Forename { get; set; } = default!;
    public string Surname { get; set; } = default!;
    public string Action { get; set; } = default!;
    public DateTime ActionDate { get; set; }
    public string? AdditionalInfo { get; set; }
}