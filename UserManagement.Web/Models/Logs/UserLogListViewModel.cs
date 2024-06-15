using System;

namespace UserManagement.Web.Models.Logs;

public class UserLogListViewModel
{
    public List<UserLogListItemViewModel> Items { get; set; } = new();
}

public class UserLogListItemViewModel
{
    public long Id { get; set; }
    public required long UserId { get; set; }
    public required string Forename { get; set; }
    public required string Surname { get; set; }
    public required string Action { get; set; }
    public DateTime ActionDate { get; set; }
    public string? AdditionalInfo { get; set; }
}