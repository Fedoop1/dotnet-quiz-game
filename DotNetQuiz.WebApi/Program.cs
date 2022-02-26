using DotNetQuiz.BLL.Interfaces;
using DotNetQuiz.BLL.Services;
using DotNetQuiz.WebApi.Infrastructure.Interfaces;
using DotNetQuiz.WebApi.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(config =>
{
    // Caution! It's absolutely not safe, never use it in production!
    config.AddPolicy("DevelopmentPolicy", config =>
    {
        config.AllowAnyHeader();
        config.AllowAnyMethod();
        config.AllowAnyOrigin();
    });
});

builder.Services.AddControllers();
builder.Services.AddSignalR();

builder.Services.AddSingleton<IQuestionHandler, QuizQuestionsHandler>();
builder.Services.AddSingleton<IRoundStatisticAnalyzer, RoundStatisticAnalyzer>();
builder.Services.AddSingleton<IQuizSessionHandlersFactory, QuizSessionHandlersFactory>();
builder.Services.AddSingleton<IQuizHandlersManager, QuizHandlersManager>();
builder.Services.AddSingleton<IQuizHubsFactory, QuizHubsFactory>();
builder.Services.AddSingleton<IQuizHubsConnectionManager, QuizHubsConnectionManager>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCors("DevelopmentPolicy");
}


app.MapControllers();

app.MapControllerRoute("DefaultApiControllerRoute", "{controller=quiz}/{action=info}");

app.Run();
