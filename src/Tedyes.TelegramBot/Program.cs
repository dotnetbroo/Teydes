using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Tedyes.TelegramBot.Services;
using Tedyes.TelegramBot.Configurations;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddHttpClient("telegram_bot_client")
                        .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
                        {
                            //Console.WriteLine(botConfig.BotToken);
                            TelegramBotClientOptions options = new(Bot.BotToken);
                            return new TelegramBotClient(options, httpClient);
                        });

                services.AddScoped<UpdateHandler>();
                services.AddScoped<ReceiverService>();
                services.AddHostedService<PollingService>();
            });
    }
}

