var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSpaStaticFiles(config => config.RootPath = "ClientApp");

var app = builder.Build();


app.UseStaticFiles();
app.UseSpaStaticFiles();

app.MapFallback(async context =>
{
    context.Response.ContentType = "text/html";
    await context.Response.SendFileAsync(Path.Combine("ClientApp", "dist", "index.html"));
});

app.Run();
