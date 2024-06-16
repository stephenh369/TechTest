using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;
using UserManagement.WebMS.Controllers;

namespace UserManagement.Data.Tests;

public class UserControllerTests
{
    [Fact]
    public void List_WhenServiceReturnsUsers_ModelMustContainUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = controller.List();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Model
            .Should().BeOfType<UserListViewModel>()
            .Which.Items.Should().BeEquivalentTo(users);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void List_WhenCalledWithActiveStatus_ReturnsUsersWithMatchingActiveStatus(bool activeStatus)
    {
        // Arrange
        var controller = CreateController();
        var activeUsers = SetupUsers(isActive: true);
        var nonActiveUsers = SetupUsers(isActive: false);

        _userService
            .Setup(s => s.FilterByActive(activeStatus))
            .Returns(activeStatus ? activeUsers : nonActiveUsers);

        // Act
        var result = controller.List(activeStatus);

        // Assert
        result.Model
            .Should().BeOfType<UserListViewModel>()
            .Which.Items.Should().BeEquivalentTo(activeStatus ? activeUsers : nonActiveUsers);
    }

    [Fact]
    public void Add_WhenCalledWithUser_RedirectsToList()
    {
        // Arrange
        var controller = CreateController();
        var user = new UserAddViewModel
        {
            Forename = "John",
            Surname = "Doe",
            Email = "jdoe@example.com",
            IsActive = true,
            DateOfBirth = new DateTime(1980, 1, 1)
        };

        // Act
        var result = controller.Add(user);

        // Assert
        result.Should().BeOfType<RedirectToActionResult>()
            .Which.ActionName.Should().Be("List");
    }

    [Fact]
    public void View_WhenCalledWithId_ReturnsViewResultWithUser()
    {
        // Arrange
        var controller = CreateController();
        var user = SetupUsers().First();
        var userView = new UserViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            IsActive = user.IsActive,
            DateOfBirth = user.DateOfBirth
        };

        _userService.Setup(s => s.GetById(user.Id)).Returns(user);

        // Act
        var result = controller.View(user.Id);

        // Assert
        result.Should().BeOfType<ViewResult>()
            .Which.Model.Should().BeOfType<UserViewModel>()
            .Which.Should().BeEquivalentTo(userView);
    }

    [Fact]
    public void Edit_WhenCalledWithUser_RedirectsToList()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsers();
        var user = users.First();
        var userEditView = new UserEditViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            IsActive = user.IsActive,
            DateOfBirth = user.DateOfBirth
        };

        _userService.Setup(s => s.GetById(user.Id)).Returns(user);

        // Act
        var result = controller.Edit(userEditView);

        // Assert
        result.Should().BeOfType<RedirectToActionResult>().Which.ActionName.Should().Be("List");
    }

    // Fails because result is null
    [Fact]
    public void DeleteConfirm_WhenCalledWithUserId_RedirectsToList()
    {
        // Arrange
        var controller = CreateController();
        var users = SetupUsers();
        var user = users.First();

        // Act
        var result = controller.DeleteConfirm(user.Id);

        // Assert
        result.Should().BeOfType<RedirectToActionResult>()
            .Which.ActionName.Should().Be("List");
    }

    private User[] SetupUsers(string forename = "Johnny", string surname = "User", string email = "juser@example.com", bool isActive = true)
    {
        var users = new[]
        {
            new User
            {
                Forename = forename,
                Surname = surname,
                Email = email,
                IsActive = isActive,
                DateOfBirth = new DateTime(1980, 1, 1)
            }
        };

        _userService
            .Setup(s => s.GetAll())
            .Returns(users);

        return users;
    }

    private readonly Mock<IUserService> _userService = new();
    private readonly Mock<ILogsService> _logsService = new();
    private UsersController CreateController() => new(_userService.Object, _logsService.Object);
}
