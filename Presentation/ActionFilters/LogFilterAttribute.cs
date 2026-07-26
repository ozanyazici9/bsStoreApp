using Entities.LogModel;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Services.Contracts;

namespace Presentation.ActionFilters;

public class LogFilterAttribute : ActionFilterAttribute
{
    private readonly ILoggerService _logger;

    public LogFilterAttribute(ILoggerService logger)
    {
        _logger = logger;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInfo(Log("OnActionExecuting", context.RouteData));
    }

    private string Log(string modelName, RouteData routeData)
    {
        var LogDetails = new LogDetails()
        {
            Controller = routeData.Values["controller"],
            Action = routeData.Values["action"],
            ModelName = modelName
        };
        
        if (routeData.Values.Count >= 3)
            LogDetails.Id = routeData.Values["id"];

        return LogDetails.ToString();
    }
}
