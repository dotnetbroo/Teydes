using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot;
using Teydes.Data.IRepositories;
using Teydes.Domain.Entities.Courses;
using Teydes.Domain.Entities.Users;
using Teydes.Service.Commons.Exceptions;
using Teydes.Service.Commons.Helpers;
using Teydes.Service.DTOs;
using Teydes.Service.Interfaces.Users;

namespace Teydes.Service.Services.Users;

public class SmsService : ISmsService
{
    private readonly IConfiguration configuration;
    private readonly IRepository<Group> groupRepository;

    public SmsService(IConfiguration configuration, IRepository<Group> groupRepository)
    {
        this.configuration = configuration;
        this.groupRepository = groupRepository;
    }

    public async Task<string> GenerateTokenAsync()
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "https://notify.eskiz.uz/api/auth/login");
        var content = new MultipartFormDataContent();
        content.Add(new StringContent($"{configuration["SmsConfig:Email"]}"), "email");
        content.Add(new StringContent($"{configuration["SmsConfig:Password"]}"), "password");
        request.Content = content;
        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();   // Check for whether send or not
        
        var token = await response.Content.ReadAsStringAsync();

        var jsonToken = JsonConvert.DeserializeObject<JObject>(token);

        var tokenGenereted = jsonToken["data"]["token"].ToString();

        return tokenGenereted;
    }


    public async Task<bool> SendAsync(Message message)
    {
        var group = await this.groupRepository.SelectAll()
            .Where(g => g.Id == message.GroupId)
            .Include(ug => ug.UserGroups)
            .ThenInclude(u => u.User)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        
        if (group is null)
            throw new CustomException(404, "Group is not found");

        if (group?.UserGroups is null)
            throw new CustomException(404, "Student not found");

        var token = await GenerateTokenAsync();
        foreach (var userGroup in group?.UserGroups)
        {
            if (userGroup.User.IsStudyForeign == false)
            {
                using var client = new HttpClient();
                using var request = new HttpRequestMessage(HttpMethod.Post, "https://notify.eskiz.uz/api/message/sms/send");

                // Add the Authorization header with the Bearer token
                request.Headers.Add("Authorization", $"Bearer {token}");

                using var content = new MultipartFormDataContent();
                content.Add(new StringContent($"{userGroup?.User?.PhoneNumber}"), "mobile_phone");
                content.Add(new StringContent($"{message.Data} \n {message.Url}"), "message");
                content.Add(new StringContent($"{configuration["SmsConfig:from"]}"), "from");
                request.Content = content;
                await client.SendAsync(request);
            }
        }

        return true;
    }

    public async Task<bool> SendMessageByTelegramAsync(long groupId, string url, string text)
    {
        var botToken = configuration["TelegramBotConfig:BotToken"];
        var botClient = new TelegramBotClient(botToken);
        InlineKeyboardMarkup inlineKeyboard = new(
        new[]
        {
            new []
            {
                InlineKeyboardButton.WithUrl("Enter the website", url),
            },
         });


        var group = await this.groupRepository.SelectAll()
        .Where(g => g.Id == groupId)
        .Include(g => g.UserGroups)
        .ThenInclude(ug => ug.User)
        .AsNoTracking()
        .FirstOrDefaultAsync();


        if (group is null)
            throw new CustomException(404, "Group is not found");

        if (group?.UserGroups is null)
            throw new CustomException(404, "Student not found");

        var foreignStudents = group.UserGroups
                .Where(userGroup => userGroup?.User?.IsStudyForeign == true  && userGroup?.User?.TelegramId != null)
                .Select(userGroup => userGroup?.User)
                .ToList();

        foreach (var foreignStudent in foreignStudents)
        {
            var result = await botClient.SendTextMessageAsync(
                chatId: foreignStudent.TelegramId,
                text: text,
                replyMarkup: inlineKeyboard);

            await Task.Delay(100);
        }

        return true;
    }

}
