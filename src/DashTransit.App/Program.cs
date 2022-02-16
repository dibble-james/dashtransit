using DashTransit.Core;
using DashTransit.EntityFramework;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

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
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Appliying migrations failed:\r\n{ex.Message}");
    }
    return;
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();