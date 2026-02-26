
using PressINK_Server_App;
using PressINK_Server_App.Data.API_Forms;
using PressINK_Server_App.Data.Auth;
using PressINK_Server_App.Services;
using PressINK_Server_App.Services.Auth.Services;
using ApexCharts;
using Blazored.LocalStorage;
using DB_SCHEMA.Data.PrinterContexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

/*builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});*/
/*builder.WebHost.ConfigureKestrel((context, serverOptions) =>
{
    serverOptions.ConfigureEndpointDefaults(listenOptions =>
    {
        // ...
    });

    serverOptions.ConfigureHttpsDefaults(listenOptions =>
    {
        // ...
    });
});*/


// Printer DB contexts
var connectionString = builder.Configuration.GetConnectionString("ConnectionPressINK")
    ?? throw new NullReferenceException("No connection string in config!");
//AdminDB
builder.Services.AddPooledDbContextFactory<AdminDBContext>((DbContextOptionsBuilder options)
    => options.UseMySQL(connectionString));
//API_Forms
builder.Services.AddPooledDbContextFactory<API_FormContext>((DbContextOptionsBuilder options)
    => options.UseMySQL(connectionString));


// Configure authorization
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddSingleton<LdapAuthenticationService>();
builder.Services.AddScoped<AuthenticationStateProvider, AuthenticationHandler>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.RequireAuthenticatedSignIn = true;
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdministratorRoleORStandard", policy => policy.RequireRole(SD.USERMODE, SD.GODEMODE));
    options.AddPolicy("RequireAuthenticatedUser", policy => policy.RequireAuthenticatedUser());
});



// Add services to the container.
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AllowAnonymousToPage("/");
});
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHostedService<APIServices>();
builder.Services.AddHostedService<PingServices>();
builder.Services.AddSingleton<PingInjectServices>();
builder.Services.AddSingleton<APIInjectServices>();
builder.Services.AddSingleton<PrinterService>();
builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddPooledDbContextFactory<PrinterContext>((DbContextOptionsBuilder options)
    => options.UseMySQL(connectionString));


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Warning()
    .WriteTo.Console()
    .WriteTo.File("logs/PressLogs-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();



var app = builder.Build();
app.UsePathBase("/press");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
//app.UseForwardedHeaders();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();

app.MapFallbackToPage("/_Host");



app.Run();





public class JwtSettings
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string SecretKey { get; set; }
    public string TokenName { get; set; }

}
