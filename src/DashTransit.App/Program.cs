using DashTransit.Core;
using DashTransit.EntityFramework;
using Fluxor;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddFluxor(opt => opt.ScanAssemblies(typeof(Program).Assembly));

builder.Services.AddDashTransit();
builder.Services.UseDashTransitEntityFramework(builder.Configuration.GetConnectionString("store"));
builder.Services.AddMassTransitHostedService();
builder.Services.AddMassTransit(bus =>
{
    bus.AddDashTransit();
    bus.UsingRabbitMq((context, rabbit) =>
    {
        rabbit.Host(builder.Configuration.GetConnectionString("transport"));
        rabbit.UseDashTransit(context);
    });
});

var app = builder.Build();

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