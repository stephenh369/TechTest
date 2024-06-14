using System;
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
    public void AddUser_WhenCalledWithUser_AddsUser()
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
        var result = controller.AddUser(user);

        // Assert
        _userService.Verify(s => s.Add(It.Is<User>(u =>
            u.Forename == user.Forename &&
            u.Surname == user.Surname &&
            u.Email == user.Email &&
            u.IsActive == user.IsActive &&
            u.DateOfBirth == user.DateOfBirth)), Times.Once);
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
                IsActive = isActive
            }
        };

        _userService
            .Setup(s => s.GetAll())
            .Returns(users);

        return users;
    }

    private readonly Mock<IUserService> _userService = new();
    private UsersController CreateController() => new(_userService.Object);
}
