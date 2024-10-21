using Teydes.Service.Interfaces.Commons;
using Teydes.Service.Services.Commons;
using Teydes.Shared.Extensions;
namespace Teydes.Web.Configurations;

public static class WebConfiguration
{
    public static void AddWeb(this WebApplicationBuilder builder)
    {
        builder.Services.AddMemoryCache();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddJwtService(builder.Configuration);
    }
}
