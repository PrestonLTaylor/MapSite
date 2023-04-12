using Serilog;
using MapSite.Endpoints;
using MapSite.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddAPIKeyAuthenticationServices();
builder.Services.AddEntityTrackingServices();

builder.Host.UseSerilog((context, config) =>
{
    config.ReadFrom.Configuration(builder.Configuration);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.UseAPIKeyAuthentication();

app.MapEntityTrackingEndpoints();

app.Run();

public partial class Program { }
