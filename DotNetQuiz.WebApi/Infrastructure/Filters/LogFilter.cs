using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DotNetQuiz.WebApi.Infrastructure.Filters
{
    public partial class LogFilter : IAsyncActionFilter
    {
        private readonly ILogger<LogFilter> logger;
        public LogFilter(ILogger<LogFilter> logger) => this.logger = logger;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controller = context.Controller as ControllerBase;
            var routeValues = controller.HttpContext.Request.RouteValues;
            var response = controller.HttpContext.Response;

            if (!routeValues.TryGetValue("controller", out var controllerName) || !routeValues.TryGetValue("action", out var controllerAction))
            {
                await next();
                return;
            }
            
            var actionArguments = string.Join(',', context.ActionArguments.Select(arg => arg.Key + " = " + arg.Value))
                .Trim();

            this.LogActionExecution(controllerName as string, controllerAction as string, actionArguments);

            await next();

            this.LogActionExecuted(controllerName as string, controllerAction as string, actionArguments, response.StatusCode.ToString());
        }

        [LoggerMessage(0, LogLevel.Information, "[{controller}] Calling action: {action}. Args: [{args}].")]
        private partial void LogActionExecution(string controller, string action, string args);

        [LoggerMessage(0, LogLevel.Information, "[{controller}] Execution end: {action}. Args: [{args}]. Status code: {statusCode}")]
        private partial void LogActionExecuted(string controller, string action, string args, string statusCode);
    }
}
