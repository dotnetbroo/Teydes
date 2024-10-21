using Tedyes.TelegramBot.Abstract;
using Microsoft.Extensions.Logging;

namespace Tedyes.TelegramBot.Services;

public class PollingService : PollingServiceBase<ReceiverService>
{
    public PollingService(IServiceProvider serviceProvider, ILogger<PollingService> logger)
        : base(serviceProvider, logger)
    {
    }
}
