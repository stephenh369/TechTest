
using System;
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
    public ViewResult List(int page = 1)
    {
        const int pageSize = 10;
        var logs = _logsService.GetAllLogs();
        var items = logs.Skip((page - 1) * pageSize)
        .Take(pageSize).Select(log => new UserLogListItemViewModel
        {
            Id = log.Id,
            Forename = log.Forename,
            Surname = log.Surname,
            UserId = log.UserId,
            Action = log.Action,
            ActionDate = log.ActionDate,
            AdditionalInfo = log.AdditionalInfo
        }).ToList();

        var totalLogs = logs.Count();
        var totalPages = (int)Math.Ceiling((double)totalLogs / pageSize);

        var model = new UserLogListViewModel
        {
            Items = items,
            CurrentPage = page,
            PageSize = pageSize,
            TotalPages = totalPages
        };

        return View(model);
    }
}