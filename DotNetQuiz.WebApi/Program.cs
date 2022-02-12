using DotNetQuiz.BLL.Interfaces;
using DotNetQuiz.BLL.Services;
using DotNetQuiz.WebApi.Infrastructure.Interfaces;
using DotNetQuiz.WebApi.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSignalR();

builder.Services.AddSingleton<IQuestionHandler, QuizQuestionsHandler>();
builder.Services.AddSingleton<IRoundStatisticAnalyzer, RoundStatisticAnalyzer>();
builder.Services.AddSingleton<IQuizSessionHandlersFactory, QuizSessionHandlersFactory>();
builder.Services.AddSingleton<IQuizHandlersManager, QuizHandlersManager>();

var app = builder.Build();

app.MapControllers();

app.Run();
