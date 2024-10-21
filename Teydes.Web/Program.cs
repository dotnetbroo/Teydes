using Serilog;
using System.Net;
using Teydes.Web.Models;
using Teydes.Data.DbContexts;
using Teydes.Service.Mappers;
using Teydes.Shared.Extensions;
using Teydes.Shared.Middlewares;
using Teydes.Web.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Teydes.Web.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddWeb();
builder.Services.AddCustomServices();
builder.Services.AddControllersWithViews();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMvc();

builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(MapperProfile));

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admins", policy =>
    policy.RequireRole("SuperAdmin, Admin"));
});

builder.Services.AddControllers(options =>
options.Conventions.Add(new RouteTokenTransformerConvention(
    new ConfigureApiUrlName()))
);

var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .Enrich.FromLogContext()
  .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
// Init accessor
app.InitAccessor();

app.UseSession();


app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseStatusCodePages(async context =>
{
    if (context.HttpContext.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
    {
        context.HttpContext.Response.Redirect("/accounts/login");
    }
});
app.UseMiddleware<JwtCookieMiddleware>();
app.UseAuthentication();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();


app.MapAreaControllerRoute(
    name: "admin",
    areaName: "Admin",
    pattern: "admin/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=accounts}/{action=login}/{id?}");

app.Run();
