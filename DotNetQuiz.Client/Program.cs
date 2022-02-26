var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSpaStaticFiles(config => config.RootPath = "ClientApp");

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
}

app.UseStaticFiles();
app.UseSpaStaticFiles();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.UseSpa(config =>
{
    config.Options.SourcePath = "ClientApp";
});

app.MapFallbackToFile("index.html");

app.Run();
