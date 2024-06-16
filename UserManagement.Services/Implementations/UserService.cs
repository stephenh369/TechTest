using System;
using System.Collections.Generic;
using System.Linq;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Domain.Implementations;

public class UserService : IUserService
{
    private readonly IDataContext _dataAccess;
    private readonly ILogsService _logsService;
    public UserService(IDataContext dataAccess, ILogsService logsService)
    {
        _dataAccess = dataAccess;
        _logsService = logsService;
    }

    /// <summary>
    /// Return users by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    public IEnumerable<User> FilterByActive(bool activeStatus) => _dataAccess.GetAll<User>().Where(user => user.IsActive == activeStatus);

    public IEnumerable<User> GetAll() => _dataAccess.GetAll<User>();

    public void Add(User user)
    {
        _dataAccess.Create(user);
        _logsService.LogUserAction(new UserActionLog
        {
            UserId = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Action = "User created",
            ActionDate = DateTime.Now,
        });
    }
    public User GetById(long id) => _dataAccess.GetById<User>(id);
    public void Edit(User updatedUser)
    {
        var changes = _dataAccess.GetChangedProperties(_dataAccess.GetById<User>(updatedUser.Id), updatedUser);

        if (!changes.Any()) return;

        _dataAccess.Update(updatedUser);
        _logsService.LogUserAction(new UserActionLog
        {
            UserId = updatedUser.Id,
            Forename = updatedUser.Forename,
            Surname = updatedUser.Surname,
            Action = "User updated",
            ActionDate = DateTime.Now,
            AdditionalInfo = $"Updated with changes: {string.Join(", ", changes)}"
        });
    }
    public void Delete(User user)
    {
        _dataAccess.Delete(user);
        _logsService.LogUserAction(new UserActionLog
        {
            UserId = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Action = "User deleted",
            ActionDate = DateTime.Now
        });
    }
}
