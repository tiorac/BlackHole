using BlackHole.Server.Hubs;
using BlackHole.Server.Services;
using BlackHole.Server.Services.Downloads;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddSingleton<ErrorHandler>();
builder.Services.AddSingleton<ServiceControl>();
builder.Services.AddHostedService<ServiceWorker>();
builder.Services.AddSingleton<ServiceRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();
app.MapHub<ServiceQueueHub>("/queue");

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

var serviceControl = app.Services.GetService<ServiceControl>();
serviceControl?.Load();

app.Run();
