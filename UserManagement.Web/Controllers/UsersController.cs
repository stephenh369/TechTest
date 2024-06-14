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

    [HttpGet("add")]
    public ViewResult Add()
    {
        return View();
    }

    [HttpPost("add")]
    public ActionResult Add(UserAddViewModel model)
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

    [HttpGet("view")]
    public ActionResult View([FromQuery] long id)
    {
        var user = _userService.GetById(id);

        var viewModel = new UserViewModel
        {
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            IsActive = user.IsActive,
            DateOfBirth = user.DateOfBirth
        };

        return View(viewModel);
    }

    [HttpGet("edit")]
    public ActionResult Edit([FromQuery] long id)
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
            return View(model);
        }

        var user = _userService.GetById(model.Id);
        if (user == null)
        {
            return NotFound();
        }

        user.Forename = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(model.Forename.ToLower());
        user.Surname = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(model.Surname.ToLower());
        user.Email = model.Email;
        user.IsActive = model.IsActive;
        user.DateOfBirth = model.DateOfBirth;

        _userService.Edit(user);

        return RedirectToAction("List");
    }

}
