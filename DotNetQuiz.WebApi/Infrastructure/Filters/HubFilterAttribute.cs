using DotNetQuiz.WebApi.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DotNetQuiz.WebApi.Infrastructure.Filters
{
    public class HubFilterAttribute : ActionFilterAttribute
    {
        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            const string sessionIdActionParamName = "sessionId";

            if (!context.ActionArguments.TryGetValue(sessionIdActionParamName, out var sessionIdObj))
            {
                context.Result = new BadRequestObjectResult(new { errorMessage = "sessionId is required!" });
            }

            var sessionId = (Guid)sessionIdObj!;

            var quizHubsConnectionManager = context.HttpContext.RequestServices.GetService<IQuizHubsConnectionManager>()!;

            if (quizHubsConnectionManager.GetQuizSessionHub(sessionId) is null)
            {
                context.Result = new BadRequestObjectResult(new
                    { errorMessage = $"Quiz session hub with id [{sessionId}] doesn't exist" });
            }

            return next();
        }
    }
}
