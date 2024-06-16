using System;
using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Implementations;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Data.Tests;

public class UserServiceTests
{
    // Tests that require getting specific user by Id via service are failing as they are returning null
    [Fact]
    public void GetById_WhenCalledWithUserId_ReturnsUser()
    {
        // Arrange
        var service = CreateService();
        var users = SetupUsers();
        var user = users.ToList()[0];

        // Act
        var result = service.GetById(user.Id);

        // Assert
        result.Should().NotBeNull();
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
        var result = service.GetAll().Where(u => u.Email == user.Email);
        result.Should().NotBeEmpty();
    }

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
    private readonly Mock<ILogsService> _logsService = new();
    private UserService CreateService() => new(_dataContext.Object, _logsService.Object);

}
