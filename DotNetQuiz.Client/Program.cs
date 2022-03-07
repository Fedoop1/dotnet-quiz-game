var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSpaStaticFiles(config => config.RootPath = "ClientApp");

var app = builder.Build();


app.UseStaticFiles();
app.UseSpaStaticFiles();

app.MapFallbackToFile(Path.Combine("ClientApp", "dist", "index.html"));

app.Run();
