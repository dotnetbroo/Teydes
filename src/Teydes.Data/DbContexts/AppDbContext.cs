using Teydes.Data.Configuration;
using Teydes.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Teydes.Domain.Entities.Assets;
using Teydes.Domain.Entities.Quizes;
using Teydes.Domain.Entities.Courses;

namespace Teydes.Data.DbContexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    public DbSet<User> Users { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Asset> Assets { get; set; }
    public DbSet<Quiz> Quizzes { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<UserGroup> UserGroups { get; set; }
    public DbSet<QuizResult> QuizResults { get; set; }
    public DbSet<Submission> Submissions { get; set; }
    public DbSet<QuestionAnswer> QuestionAnswers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Seed Model
        modelBuilder.ApplyConfiguration(new SuperAdminConfiguration());
        #endregion
    }
}
