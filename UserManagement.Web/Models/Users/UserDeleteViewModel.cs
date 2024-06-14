namespace UserManagement.Web.Models.Users
{
    public class UserDeleteViewModel
    {
        public long Id { get; set; }
        public string Forename { get; set; } = default!;
        public string Surname { get; set; } = default!;
    }
}