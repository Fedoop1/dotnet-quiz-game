var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSpaStaticFiles(config => config.RootPath = "ClientApp/dist");

builder.Services.AddMvc(opt => opt.EnableEndpointRouting = false);

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.UseSpa((config) =>
{
    config.Options.SourcePath = "ClientApp";

    if (app.Environment.IsDevelopment())
    {
        config.UseProxyToSpaDevelopmentServer("http://localhost:4200");
    }
});
