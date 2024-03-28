using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using FAQCompaniesChat.Data;
using FAQCompaniesChat.Hubs;
using FAQCompaniesChat.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddSingleton<ChatService>();

var isDevelopment = string.Equals(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"), "development", StringComparison.InvariantCultureIgnoreCase);
if (isDevelopment)
{
    builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("./Properties/appsettings.Secrets.json");
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

// app.MapBlazorHub();
// app.MapFallbackToPage("/_Host");

app.UseEndpoints(endpoints =>
{
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/_Host");
    endpoints.MapHub<BlazorChatSampleHub>(BlazorChatSampleHub.HubUrl);
});


app.Run();
