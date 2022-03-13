using System.Net.NetworkInformation;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSpaStaticFiles(config => config.RootPath = "ClientApp/dist");

var app = builder.Build();

// In production config we'll serve static files from dist folder
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

// [emergency case] When we can't serve a development server node, but we have a static copy in dist folder
app.MapFallbackToFile("index.html", new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.ContentRootPath, "ClientApp", "dist"))
});

app.Run();
