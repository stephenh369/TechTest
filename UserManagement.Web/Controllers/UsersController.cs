using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    private readonly ILogsService _logsService;
    public UsersController(IUserService userService, ILogsService logsService)
    {
        _userService = userService;
        _logsService = logsService;
    }
    [HttpGet]
    public ViewResult List(bool? activeStatus = null)
    {
        var users = activeStatus.HasValue ? _userService.FilterByActive(activeStatus.Value) : _userService.GetAll();
        var items = users.Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            IsActive = p.IsActive,
            DateOfBirth = p.DateOfBirth
        });

        var model = new UserListViewModel
        {
            Items = items.ToList()
        };

        return View(model);
    }

    [HttpGet("add-user")]
    public ViewResult AddUser()
    {
        return View();
    }

    [HttpPost("add")]
    public ActionResult Add(UserAddViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("AddUser", model);
        }

        var user = new User
        {
            Forename = char.ToUpper(model.Forename[0]) + model.Forename.Substring(1),
            Surname = char.ToUpper(model.Surname[0]) + model.Surname.Substring(1),
            Email = model.Email,
            IsActive = model.IsActive,
            DateOfBirth = model.DateOfBirth
        };

        _userService.Add(user);

        return RedirectToAction("List");
    }

    [HttpGet("view")]
    public ViewResult View([FromQuery] long id)
    {
        var user = _userService.GetById(id);

        var userLogs = _logsService.GetAllLogs().Where(log => log.UserId == user.Id);

        var viewModel = new UserViewModel
        {
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            IsActive = user.IsActive,
            DateOfBirth = user.DateOfBirth,
            UserActionLogs = userLogs.Select(log => new UserActionLog
            {
                Id = log.Id,
                UserId = log.UserId,
                Forename = log.Forename,
                Surname = log.Surname,
                Action = log.Action,
                ActionDate = log.ActionDate,
                AdditionalInfo = log.AdditionalInfo
            }).ToList()
        };

        return View(viewModel);
    }

    [HttpGet("edit-user")]
    public ViewResult EditUser([FromQuery] long id)
    {
        var user = _userService.GetById(id);

        var editModel = new UserEditViewModel
        {
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            IsActive = user.IsActive,
            DateOfBirth = user.DateOfBirth
        };

        return View(editModel);
    }

    [HttpPost("edit")]
    public ActionResult Edit(UserEditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("EditUser", model);
        }

        var user = _userService.GetById(model.Id);
        if (user == null)
        {
            return NotFound();
        }

        user.Forename = char.ToUpper(model.Forename[0]) + model.Forename.Substring(1);
        user.Surname = char.ToUpper(model.Surname[0]) + model.Surname.Substring(1);
        user.Email = model.Email;
        user.IsActive = model.IsActive;
        user.DateOfBirth = model.DateOfBirth;

        _userService.Edit(user);

        return RedirectToAction("List");
    }

    [HttpGet("delete-confirm")]
    public ViewResult DeleteConfirm([FromQuery] long id)
    {
        var user = _userService.GetById(id);

        var deleteModel = new UserDeleteViewModel
        {
            Forename = user.Forename,
            Surname = user.Surname,
        };

        return View(deleteModel);
    }

    [HttpPost("delete")]
    public ActionResult Delete(long id)
    {
        var user = _userService.GetById(id);
        if (user == null)
        {
            return NotFound();
        }

        _userService.Delete(user);

        return RedirectToAction("List");
    }
}
