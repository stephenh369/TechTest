using System.Collections.Generic;
using System.Linq;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Domain.Implementations;

public class UserService : IUserService
{
    private readonly IDataContext _dataAccess;
    public UserService(IDataContext dataAccess) => _dataAccess = dataAccess;

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
    }
    public User GetById(long id)
    {
        return _dataAccess.GetById<User>(id);
    }
    public void Edit(User updatedUser)
    {
        _dataAccess.Update(updatedUser);
    }
    public void Delete(User user)
    {
        _dataAccess.Delete(user);
    }
}
