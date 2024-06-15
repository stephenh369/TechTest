using System.Collections.Generic;
using UserManagement.Models;

namespace UserManagement.Services.Domain.Interfaces;

public interface ILogsService
{
    void LogUserAction(UserActionLog userActionLog);
    IEnumerable<UserActionLog> GetAllLogs();
}