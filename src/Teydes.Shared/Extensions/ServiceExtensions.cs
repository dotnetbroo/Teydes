using Teydes.Data.Repositories;
using Teydes.Data.IRepositories;
using Teydes.Service.Services.Users;
using Teydes.Service.Services.Groups;
using Teydes.Service.Services.Assets;
using Teydes.Service.Interfaces.Users;
using Teydes.Service.Services.Courses;
using Teydes.Service.Services.Quizzes;
using Teydes.Service.Services.Commons;
using Teydes.Service.Services.Answers;
using Teydes.Service.Services.Accounts;
using Teydes.Service.Interfaces.Assets;
using Teydes.Service.Interfaces.Groups;
using Teydes.Service.Interfaces.Answers;
using Teydes.Service.Services.Questions;
using Teydes.Service.Interfaces.Commons;
using Teydes.Service.Interfaces.Courses;
using Teydes.Service.Interfaces.Quizzes;
using Teydes.Service.Interfaces.Accounts;
using Teydes.Service.Services.UserGroups;
using Teydes.Service.Interfaces.Questions;
using Teydes.Service.Services.QuizResults;
using Teydes.Service.Services.Submissions;
using Teydes.Service.Interfaces.UserGroups;
using Teydes.Service.Interfaces.Submissions;
using Teydes.Service.Interfaces.QuizResults;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;   
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Teydes.Shared.Extensions;

public static class ServiceExtensions
{
    public static void AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        services.AddScoped<ISmsService, SmsService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IQuizService, QuizService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IGroupService, GroupService>();
        services.AddScoped<IAssetService, AssetService>();
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IQuestionService, QuestionService>();
        services.AddScoped<IUserGroupService, UserGroupService>();
        services.AddScoped<IQuizResultService, QuizResultService>();
        services.AddScoped<ISubmissionService, SubmissionService>();
        services.AddScoped<IQuestionAnswerService, QuestionAnswerService>();

        //services.AddScoped<IConfigurationSection>(provider =>
        //{
        //    var configuration = provider.GetRequiredService<IConfiguration>();
        //    return configuration.GetSection("Jwt");
        //});

    }


    public static void AddJwtService(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            var Key = Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]);
            o.SaveToken = true;
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["JWT:Issuer"],
                ValidAudience = configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Key),
                ClockSkew = TimeSpan.FromMinutes(1)
            };
        });
    }
}
