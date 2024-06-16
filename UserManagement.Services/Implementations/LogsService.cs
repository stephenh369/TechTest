
using System.Collections.Generic;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Domain.Implementations;

public class LogsService : ILogsService
{
    private readonly IDataContext _dataAccess;
    public LogsService(IDataContext dataAccess) => _dataAccess = dataAccess;
    public void LogUserAction(UserActionLog userActionLog) => _dataAccess.Create(userActionLog);
    public IEnumerable<UserActionLog> GetAllLogs() =>
        _dataAccess.GetAll<UserActionLog>();
}