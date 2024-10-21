using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teydes.Domain.Entities.Users;
using Teydes.Domain.Enums;

namespace Teydes.Data.Configuration;

public class SuperAdminConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasData(new User()
        {
            Id = 9,
            FirstName = "Javohir",
            LastName = "Boyaliyev",
            PhoneNumber = "+998889084000",
            Password = "$2a$11$Kv96RMO8xBWAdwgo8kYOD./o6LAhS9iZnbcYbTrUmYONN.lN4vj7m",
            Salt = "0b345b73-5e3c-47f3-8c39-79414f7fe1e3",
            Role = UserRole.SuperAdmin,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        });
    }
}
