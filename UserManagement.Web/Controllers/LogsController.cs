
using System.Linq;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Logs;

namespace UserManagement.Web.Controllers;

[Route("logs")]
public class LogsController : Controller
{
    private readonly ILogsService _logsService;
    public LogsController(ILogsService logsService) => _logsService = logsService;

    [HttpGet]
    public ViewResult List()
    {
        var logs = _logsService.GetAllLogs();
        var items = logs.Select(log => new UserLogListItemViewModel
        {
            Id = log.Id,
            Forename = log.Forename,
            Surname = log.Surname,
            UserId = log.UserId,
            Action = log.Action,
            ActionDate = log.ActionDate,
            AdditionalInfo = log.AdditionalInfo
        });

        var model = new UserLogListViewModel
        {
            Items = items.ToList()
        };

        return View(model);
    }
}