using DotNetQuiz.WebApi.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DotNetQuiz.WebApi.Infrastructure.Filters
{
    public class SessionFilterAttribute : ActionFilterAttribute
    {
        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            const string sessionIdActionParamName = "sessionId";

            if(!context.ActionArguments.TryGetValue(sessionIdActionParamName, out var sessionIdObj))
            {
                context.Result = new BadRequestObjectResult(new { errorMessage = "sessionId is required!"});
                return Task.CompletedTask;
            }

            var sessionId = (Guid)sessionIdObj!;

            var quizHandlersManager = context.HttpContext.RequestServices.GetService<IQuizHandlersManager>()!;

            if (quizHandlersManager.GetSessionHandler(sessionId) is null)
            {
                context.Result = new BadRequestObjectResult(new
                    { errorMessage = $"Quiz session with id [{sessionId}] doesn't exist" });
                return Task.CompletedTask;
            }

            return next();
        }
    }
}
