using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DotNetQuiz.WebApi.Infrastructure.Filters
{
    public class ExceptionHandlerFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var environment = context.HttpContext.RequestServices.GetService<IWebHostEnvironment>();
            var logger = context.HttpContext.RequestServices.GetService<ILogger<ExceptionHandlerFilterAttribute>>();

            string problemDetails =
                environment.IsDevelopment() ? context.Exception.Message : "Please contact support and let us know to what happened";

            logger.LogError(context.Exception, $"An exception occurred in {context.Exception.TargetSite.Name}");

            context.Result = new BadRequestObjectResult(new ProblemDetails()
            {
                Title = "An error occurred",
                Status = StatusCodes.Status400BadRequest,
                Detail = problemDetails, 
                Instance = $"DotNetQuizGame:{context.Exception.HResult}"
            });

            context.ExceptionHandled = true;
        }
    }
}
