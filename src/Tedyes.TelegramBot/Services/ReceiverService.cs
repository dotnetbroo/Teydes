using Microsoft.Extensions.Logging;
using Tedyes.TelegramBot.Abstract;
using Telegram.Bot;

namespace Tedyes.TelegramBot.Services;

public class ReceiverService : ReceiverServiceBase<UpdateHandler>
{
    public ReceiverService(
        ITelegramBotClient botClient,
        UpdateHandler updateHandler,
        ILogger<ReceiverServiceBase<UpdateHandler>> logger)
        : base(botClient, updateHandler, logger)
    {
    }
}