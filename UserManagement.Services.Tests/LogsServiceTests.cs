using System;
using System.Linq;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Implementations;

namespace UserManagement.Services.Tests
{
    public class LogsServiceTests
    {
        [Fact]
        public void GetAllLogs_WhenCalled_ReturnsAllLogs()
        {
            // Arrange
            var service = CreateService();
            var expectedLogs = SetupLogs();

            // Act
            var result = service.GetAllLogs();

            // Assert
            result.Should().BeEquivalentTo(expectedLogs);
        }

        // Tests that require getting specific log by Id via service are failing
        [Fact]
        public void LogUserAction_WhenCalled_AddsLog()
        {
            // Arrange
            var service = CreateService();
            var log = new UserActionLog
            {
                UserId = 1,
                Forename = "Test",
                Surname = "User",
                Action = "User created",
                ActionDate = DateTime.Now
            };

            // Act
            service.LogUserAction(log);

            // Assert
            var result = service.GetAllLogs().FirstOrDefault();
            result.Should().BeEquivalentTo(log);
        }

        private IQueryable<UserActionLog> SetupLogs()
        {
            var logs = new[]
            {
            new UserActionLog
            {
                UserId = 1,
                Forename = "Test",
                Surname = "User",
                Action = "User created",
                ActionDate = DateTime.Now
            }
        }.AsQueryable();

            _dataContext
                .Setup(s => s.GetAll<UserActionLog>())
                .Returns(logs);

            return logs;
        }
        private readonly Mock<IDataContext> _dataContext = new();
        private LogsService CreateService() => new(_dataContext.Object);
    }
}