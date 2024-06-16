using UserManagement.Models;

namespace UserManagement.Web.Models.Users
{
    public class UserViewModel : UserModel {
        public List<UserActionLog> UserActionLogs { get; set; } = [];
     }
}
