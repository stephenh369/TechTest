using System;
using System.Linq;
using UserManagement.Models;

namespace UserManagement.Data.Tests;

public class DataContextTests
{

    [Fact]
    public void Create_WhenCalledWithUser_CreatesUser()
    {
        // Arrange
        var context = CreateContext();
        var user = new User
        {
            Forename = "John",
            Surname = "Doe",
            Email = "jdoe@example.com",
            IsActive = true,
            DateOfBirth = new DateTime(1980, 1, 1)
        };

        // Act
        context.Create(user);

        // Assert
        var result = context.GetAll<User>();
        result.Should().Contain(u =>
            u.Forename == user.Forename &&
            u.Surname == user.Surname &&
            u.Email == user.Email &&
            u.IsActive == user.IsActive &&
            u.DateOfBirth == user.DateOfBirth);
    }

    [Fact]
    public void GetAll_WhenNewEntityAdded_MustIncludeNewEntity()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var context = CreateContext();

        var entity = new User
        {
            Forename = "Brand New",
            Surname = "User",
            Email = "brandnewuser@example.com"
        };
        context.Create(entity);

        // Act: Invokes the method under test with the arranged parameters.
        var result = context.GetAll<User>();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result
            .Should().Contain(s => s.Email == entity.Email)
            .Which.Should().BeEquivalentTo(entity);
    }

    [Fact]
    public void GetAll_WhenDeleted_MustNotIncludeDeletedEntity()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var context = CreateContext();
        var entity = context.GetAll<User>().First();
        context.Delete(entity);

        // Act: Invokes the method under test with the arranged parameters.
        var result = context.GetAll<User>();

        // Assert: Verifies that the action of the method under test behaves as expected.
        result.Should().NotContain(s => s.Email == entity.Email);
    }

    private DataContext CreateContext() => new();
}
