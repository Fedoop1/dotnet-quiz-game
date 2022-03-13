var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSpaStaticFiles(config => config.RootPath = "ClientApp/dist");

var app = builder.Build();

app.UseStaticFiles();

if(!app.Environment.IsDevelopment())
{
    app.UseSpaStaticFiles();
}

app.UseSpa((config) =>
{
    config.Options.SourcePath = "ClientApp";

    if (app.Environment.IsDevelopment())
    {
        config.UseProxyToSpaDevelopmentServer("http://localhost:4200");
    }
});

app.MapFallbackToFile("index.html");

app.Run();
