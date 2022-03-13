using System.Text.Json;
using DotNetQuiz.BLL.Interfaces;
using DotNetQuiz.BLL.Services;
using DotNetQuiz.WebApi.Infrastructure.Filters;
using DotNetQuiz.WebApi.Infrastructure.Hubs;
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
        config.WithOrigins("http://localhost:4200", "http://localhost:6000", "https://localhost:6001");
        config.AllowCredentials();
    });
});

builder.Services.AddControllers(config =>
    {
        config.Filters.Add<LogFilter>();
    })
    .AddJsonOptions(config =>
    {
        config.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        config.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

builder.Services.AddSignalR(config =>
{
    config.EnableDetailedErrors = true;
    config.ClientTimeoutInterval = TimeSpan.FromMinutes(15);
});

builder.Services.AddSingleton<IQuestionHandler, QuizQuestionsHandler>();
builder.Services.AddSingleton<IRoundStatisticAnalyzer, RoundStatisticAnalyzer>();
builder.Services.AddSingleton<IQuizSessionHandlersFactory, QuizSessionHandlersFactory>();
builder.Services.AddSingleton<IQuizHandlersManager, QuizHandlersManager>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCors("DevelopmentPolicy");
}

app.MapHub<QuizHub>("quiz/{sessionId:guid}/{nickName}/{isHost:bool}");

app.MapControllerRoute("DefaultApiControllerRoute", "{controller=quiz}/{action=info}");

app.Run();
