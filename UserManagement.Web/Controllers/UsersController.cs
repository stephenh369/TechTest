using System.Globalization;
using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;

[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService) => _userService = userService;

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

    [HttpPost("add-user")]
    public ActionResult AddUser(UserAddViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = new User
        {
            Forename = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(model.Forename.ToLower()),
            Surname = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(model.Surname.ToLower()),
            Email = model.Email,
            IsActive = model.IsActive,
            DateOfBirth = model.DateOfBirth
        };

        _userService.Add(user);

        return RedirectToAction("List");
    }

}
