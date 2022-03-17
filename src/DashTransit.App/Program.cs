using DashTransit.Core;
using DashTransit.EntityFramework;
using Havit.Blazor.Components.Web;
using MassTransit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;

if (args.FirstOrDefault()?.Equals("--ready", StringComparison.InvariantCultureIgnoreCase) == true)
{
    try
    {
        using var client = new HttpClient();
        var result = await client.GetStringAsync("http://localhost/health/ready");
        Console.WriteLine(result);
        return result.Trim().Equals("Healthy", StringComparison.InvariantCultureIgnoreCase)
            ? 0
            : 1;
    }
    catch(Exception ex)
    {
        Console.Error.WriteLine(ex);
        return 1;
    }
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHxServices();
builder.Services.AddHxMessenger();

builder.Services.AddHealthChecks()
    .AddDbContextCheck<DashTransitContext>(tags: new[] { "ready" });

builder.Services.AddDashTransit();
builder.Services.UseDashTransitEntityFramework(
    builder.Configuration.GetValue<string>("store:provider"),
    builder.Configuration.GetValue<string>("store:connection"));
builder.Services.AddMassTransitHostedService();
builder.Services.AddMassTransit(bus =>
{
    bus.AddDashTransit();
    bus.UsingRabbitMq((context, rabbit) =>
    {
        rabbit.Host(builder.Configuration.GetValue<string>("transport:connection"));
        rabbit.UseDashTransit(context);
    });
});

var app = builder.Build();

if (args.FirstOrDefault()?.Equals("migrate", StringComparison.InvariantCultureIgnoreCase) == true)
{
    using var scope = app.Services.CreateScope();
    var database = scope.ServiceProvider.GetRequiredService<DashTransitContext>().Database;
    Console.Out.WriteLine($"Applying migrations for DashTransit");
    try
    {
        database.Migrate();
        Console.Out.WriteLine("Migrations applied successfully");
        return 0;
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Appliying migrations failed:\r\n{ex.Message}");
        return -1;
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = (check) => check.Tags.Contains("ready"),
});
app.MapHealthChecks("/health/live", new HealthCheckOptions());

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

return 0;