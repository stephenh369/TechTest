using System;
using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Implementations;

namespace UserManagement.Data.Tests;

public class UserServiceTests
{
    [Fact]
    public void GetAll_WhenContextReturnsEntities_MustReturnSameEntities()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var service = CreateService();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = service.GetAll();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().BeSameAs(users);
    }

    [Fact]
    public void Add_WhenCalledWithUser_AddsUser()
    {
        // Arrange
        var service = CreateService();
        var user = new User
        {
            Forename = "John",
            Surname = "Doe",
            Email = "jdoe@example.com",
            IsActive = true,
            DateOfBirth = new DateTime(1980, 1, 1)
        };

        // Act
        service.Add(user);

        // Assert
        _dataContext.Verify(s => s.Create(It.Is<User>(u =>
            u.Forename == user.Forename &&
            u.Surname == user.Surname &&
            u.Email == user.Email &&
            u.IsActive == user.IsActive &&
            u.DateOfBirth == user.DateOfBirth)), Times.Once);
    }

    private IQueryable<User> SetupUsers(string forename = "Johnny", string surname = "User", string email = "juser@example.com", bool isActive = true)
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
        }.AsQueryable();

        _dataContext
            .Setup(s => s.GetAll<User>())
            .Returns(users);

        return users;
    }

    private readonly Mock<IDataContext> _dataContext = new();
    private UserService CreateService() => new(_dataContext.Object);
}
