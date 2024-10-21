using Serilog;
using Teydes.Api.Extensions;
using Teydes.Data.DbContexts;
using Teydes.Service.Mappers;
using Teydes.Shared.Extensions;
using Teydes.Shared.Middlewares;
using Microsoft.EntityFrameworkCore;
using Teydes.Service.Commons.Helpers;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Teydes.Shared.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Set Database Configuration
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
// Configure Servervice
builder.Services.AddCustomServices();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(typeof(MapperProfile));

// CORS
builder.Services.ConfigureCors();

// swagger set up
builder.Services.AddSwaggerService();
// JWT service
builder.Services.AddJwtService(builder.Configuration);

// Logger
var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .Enrich.FromLogContext()
  .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admins", policy =>
    {
        policy.RequireRole("Admin", "SuperAdmin");
    });
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("TeachersAndAdmins", policy =>
    {
        policy.RequireRole("Teacher","Admin", "SuperAdmin");
    });
});


// Convert Api Url name to dashcase
builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(new ConfigureApiUrlName()));
});

builder.Services.AddMemoryCache();

var app = builder.Build();

// Getting wwwroot path
EnvironmentHelper.WebRootPath = Path.GetFullPath("wwwroot");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{   
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseStaticFiles();
app.UseHttpsRedirection();

// Init accessor
app.InitAccessor();

//app.InitAccessor();
app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
