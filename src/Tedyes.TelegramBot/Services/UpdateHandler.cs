using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Tedyes.TelegramBot.Models;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Teydes.Service.DTOs.Users;
using Teydes.Service.Services.Users;

namespace Tedyes.TelegramBot.Services;

public class UpdateHandler : IUpdateHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<UpdateHandler> _logger;
    public  string backUrl = "https://api.quizhaad.uz/api/users";

    public UpdateHandler(ITelegramBotClient botClient, ILogger<UpdateHandler> logger)
    {
        this._botClient = botClient;
        this._logger = logger;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient _, Update update, CancellationToken cancellationToken)
    {
        var handler = update switch
        {
            { Message: { } message } => BotOnMessageReceived(message, cancellationToken),
            { EditedMessage: { } message } => BotOnMessageReceived(message, cancellationToken),
            { CallbackQuery: { } callbackQuery } => BotOnCallbackQueryReceived(callbackQuery, cancellationToken),
            _ => UnknownUpdateHandlerAsync(update, cancellationToken)
        };

        await handler;
    }

    public async Task<bool> UserAlredyExistsCheck(long telegramId)
    {
        using (HttpClient httpClient = new HttpClient())
        {
            try
            {
                string apiUrl = $"https://api.quizhaad.uz/api/users/CheckUser/{telegramId}";

                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }

    public async Task<bool> SendUrlToUser(long telegraId, string text, string url)
    {
        ITelegramBotClient bot = this._botClient;
        InlineKeyboardMarkup inlineKeyboard = new(
            new[]
                {
                new []
                    {
                        InlineKeyboardButton.WithUrl("Saytga kirish", url),
                    },
                });

        var result = await this._botClient.SendTextMessageAsync(
            chatId: telegraId,
            text: text,
            replyMarkup: inlineKeyboard);
        if (result is not null)
            return true;
        return false;
    }

    private async Task BotOnMessageReceived(Message message, CancellationToken cancellationToken)
    {
        if (message.Contact is not null)
        {
            var confirmAction = AskIfContactIsYours(this._botClient, message, message.Contact, cancellationToken);
            Message confirmMessage = await confirmAction;
            _logger.LogInformation("The confirmation message was sent with id: {ConfirmMessageId}", confirmMessage.MessageId);
        }

        _logger.LogInformation("Receive message type: {MessageType}", message.Type);
        if (message.Text is not { } messageText)
            return;

        var action = messageText.Split(' ')[0] switch
        {
            "/help" => SendInlineKeyboard(this._botClient, message, cancellationToken),
            "/start" => RequestContact(this._botClient, message, cancellationToken),
            _ => Usage(this._botClient, message, cancellationToken)
        };
        Message sentMessage = await action;
        _logger.LogInformation("The message was sent with id: {SentMessageId}", sentMessage.MessageId);

        

        async Task<Message> SendInlineKeyboard(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            InlineKeyboardMarkup inlineKeyboard = new(
                new[]
                {
                    new []
                    {
                        InlineKeyboardButton.WithUrl("Saytga kirish", "https://www.google.com"),
                    },
                });

            return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Click this button.",
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }

        async Task<Message> AskIfContactIsYours(ITelegramBotClient botClient, Message message, Contact contact, CancellationToken cancellationToken)
        {

            InlineKeyboardMarkup replyKeyboardMarkupNumberConfirm = new(
                new[]
                {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("✅Yes", $"phoneConfirmed_{contact.PhoneNumber}"),
                        InlineKeyboardButton.WithCallbackData("❌No", "phoneUnConfirmed"),
                    },
                });

            return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"Confirm your phone number? {contact.PhoneNumber}",
                replyMarkup: replyKeyboardMarkupNumberConfirm,
                cancellationToken: cancellationToken
            );
        }

        async Task<Message> RequestContact(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            var userAlredyExists = await UserAlredyExistsCheck(message.From.Id);
            if (!userAlredyExists)
            {
                ReplyKeyboardMarkup RequestReplyKeyboard = new(
                new[]
                {
                    KeyboardButton.WithRequestContact("Contact"),
                })
                {
                    ResizeKeyboard = true,
                };

                return await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Please send me your contact using button or input +...",
                    replyMarkup: RequestReplyKeyboard,
                    cancellationToken: cancellationToken);
            }
            else
            {
                return await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Welcome.",
                    cancellationToken: cancellationToken);
            }
            
        }

        async Task<Message> Usage(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            var userAlredyExists = await UserAlredyExistsCheck(message.From.Id);
            if (!userAlredyExists)
            {
                string userInput = message.Text;
                string phoneNumberPattern = @"^\+\d{4,15}([\s-]?\d+)*$";

                InlineKeyboardMarkup replyKeyboardMarkupNumberConfirm = new(
                    new[]
                    {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("✅Yes", $"phoneConfirmed_{userInput}"),
                        InlineKeyboardButton.WithCallbackData("❌No", "phoneUnConfirmed"),
                    },
                    });

                if (Regex.IsMatch(userInput, phoneNumberPattern))
                {
                    return await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: $"Confirm your phone number? {userInput}",
                        replyMarkup: replyKeyboardMarkupNumberConfirm,
                        cancellationToken: cancellationToken);
                }
                else
                {
                    const string welcomeMessage = "Welcome.";
                    return await botClient.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: welcomeMessage,
                        replyMarkup: new ReplyKeyboardRemove(),
                        cancellationToken: cancellationToken);
                }
            }
            else
            {
                return await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Welcome.",
                    cancellationToken: cancellationToken);
            }

        }
    }

    private async Task BotOnCallbackQueryReceived(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received inline keyboard callback from: {CallbackQueryId}", callbackQuery.Id);

        await this._botClient.AnswerCallbackQueryAsync(
            callbackQueryId: callbackQuery.Id,
            text: $"Received {callbackQuery.Data}",
            cancellationToken: cancellationToken);

        await this._botClient.DeleteMessageAsync(
            chatId: callbackQuery.Message!.Chat.Id,
            messageId: callbackQuery.Message.MessageId
            );

        if (callbackQuery.Data.StartsWith("phoneConfirmed_"))
        {
            string phoneNumber = callbackQuery.Data.Substring("phoneConfirmed_".Length);
            if (phoneNumber.StartsWith("+"))
            {
                phoneNumber = phoneNumber.Substring(1);
                using (HttpClient httpClient = new HttpClient())
                {
                    try
                    {
                        string apiValidPhoneUrl = $"https://api.quizhaad.uz/api/users/phone-number?phoneNumber=%2B{phoneNumber}";
                        HttpResponseMessage responseUserData = await httpClient.GetAsync(apiValidPhoneUrl);

                        if (responseUserData.IsSuccessStatusCode)
                        {
                            string responseData = await responseUserData.Content.ReadAsStringAsync();
                            var userResult = JsonConvert.DeserializeObject<UserDataResponse>(responseData);
                            int userId = userResult.data.id;
                            long telegramId = callbackQuery.From.Id;

                            string apiUpdateTelegramId = $"https://api.quizhaad.uz/api/users/Users/{userId}/{telegramId}";
                            var contentData = new
                            {
                                UserId = userId,
                                TelegramId = telegramId
                            };
                            string json = JsonConvert.SerializeObject(contentData);
                            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                            HttpResponseMessage updateTelegramId = await httpClient.PatchAsync(apiUpdateTelegramId, httpContent);



                            await this._botClient.SendTextMessageAsync(
                                chatId: callbackQuery.Message!.Chat.Id,
                                text: "You have successfully registered. we will let you know soon.",
                                replyMarkup: new ReplyKeyboardRemove(),
                                cancellationToken: cancellationToken);
                        }
                        else
                        {
                            await this._botClient.SendTextMessageAsync(
                                chatId: callbackQuery.Message!.Chat.Id,
                                text: "please register with this phone number.",
                                replyMarkup: new ReplyKeyboardRemove(),
                                cancellationToken: cancellationToken);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred: {ex.Message}");
                    }
                }
            }
            else
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    try
                    {
                        string apiValidPhoneUrl = $"https://api.quizhaad.uz/api/users/phone-number?phoneNumber={phoneNumber}";
                        HttpResponseMessage responseUserData = await httpClient.GetAsync(apiValidPhoneUrl);

                        if (responseUserData.IsSuccessStatusCode)
                        {
                            string responseData = await responseUserData.Content.ReadAsStringAsync();
                            var userResult = JsonConvert.DeserializeObject<UserDataResponse>(responseData);
                            int userId = userResult.data.id;
                            long telegramId = callbackQuery.From.Id;

                            string apiUpdateTelegramId = $"https://api.quizhaad.uz/api/users/Users/{userId}/{telegramId}";
                            var contentData = new
                            {
                                UserId = userId,
                                TelegramId = telegramId
                            };
                            string json = JsonConvert.SerializeObject(contentData);
                            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                            HttpResponseMessage updateTelegramId = await httpClient.PatchAsync(apiUpdateTelegramId, httpContent);



                            await this._botClient.SendTextMessageAsync(
                                chatId: callbackQuery.Message!.Chat.Id,
                                text: "You have successfully registered. we will let you know soon.",
                                replyMarkup: new ReplyKeyboardRemove(),
                                cancellationToken: cancellationToken);
                        }
                        else
                        {
                            await this._botClient.SendTextMessageAsync(
                                chatId: callbackQuery.Message!.Chat.Id,
                                text: "please register with this phone number.",
                                replyMarkup: new ReplyKeyboardRemove(),
                                cancellationToken: cancellationToken);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred: {ex.Message}");
                    }
                }

            }
            
        }

        else if (callbackQuery.Data.Equals("phoneUnConfirmed"))
        {
            await this._botClient.SendTextMessageAsync(
                chatId: callbackQuery.Message!.Chat.Id,
                text: "Please send me your contact using button or input +...",
                cancellationToken: cancellationToken);
        }
    }

    private Task UnknownUpdateHandlerAsync(Update update, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Unknown update type: {UpdateType}", update.Type);
        return Task.CompletedTask;
    }

    public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        _logger.LogInformation("HandleError: {ErrorMessage}", ErrorMessage);

        // Cooldown in case of network connection error
        if (exception is RequestException)
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
    }
}