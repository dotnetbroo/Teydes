using AutoMapper;
using Telegram.Bot;
using Teydes.Service.DTOs.Users;
using Teydes.Data.IRepositories;
using Teydes.Service.DTOs.Groups;
using Teydes.Domain.Configurations;
using Teydes.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Teydes.Service.Commons.Helpers;
using Teydes.Service.Interfaces.Users;
using Telegram.Bot.Types.ReplyMarkups;
using Teydes.Service.Commons.Exceptions;
using Teydes.Service.Commons.Extensions;
using Microsoft.Extensions.Configuration;
using Teydes.Domain.Enums;
using Teydes.Service.Interfaces.UserGroups;
using Teydes.Domain.Entities.Courses;
using Teydes.Service.DTOs.Quizzes;

namespace Teydes.Service.Services.Users;

public class UserService : IUserService
{
    private readonly IMapper mapper;
    private readonly IConfiguration configuration;
    private readonly IUserService userService;
    private readonly IRepository<User> userRepository;
    private readonly IRepository<UserGroup> userGroupRepository;

    public UserService(
        IMapper mapper,
        IConfiguration configuration,
        IRepository<User> userRepository,
        IRepository<UserGroup> userGroupRepository)
    {
        this.mapper = mapper;
        this.configuration = configuration;
        this.userRepository = userRepository;
        this.userGroupRepository = userGroupRepository;
    }

    public async Task<UserForResultDto> AddAsync(UserForCreationDto dto)
    {
        var user = await this.userRepository.SelectAsync(u => u.PhoneNumber == dto.PhoneNumber);
        if (user is not null)
            throw new CustomException(403, "User is already exists");

        var hasherResult = PasswordHelper.Hash(dto.Password);
        var mapped = this.mapper.Map<User>(dto);

        mapped.CreatedAt = TimeHelper.GetCurrentServerTime();
        mapped.Salt = hasherResult.Salt;
        mapped.Password = hasherResult.Hash;

        var result = await this.userRepository.InsertAsync(mapped);
        await this.userRepository.SaveAsync();
        return this.mapper.Map<UserForResultDto>(result);
    }

    public async Task<bool> ChangePasswordAsync(long id, UserForChangePasswordDto dto)
    {
        var user = await this.userRepository.SelectAsync(u => u.Id == id);
        if (user is null || !PasswordHelper.Verify(dto.OldPassword, user.Salt, user.Password))
            throw new CustomException(404, "User or Password is incorrect");
        else if (dto.NewPassword != dto.ConfirmPassword)
            throw new CustomException(400, "New password and confir password aren't equal");

        var hash = PasswordHelper.Hash(dto.ConfirmPassword);
        user.Salt = hash.Salt;
        user.Password = hash.Hash;
        var updated = this.userRepository.Update(user);

        return await this.userRepository.SaveAsync();

    }

    public async Task<bool> ForgetPasswordAsync(string PhoneNumber, string NewPassword, string ConfirmPassword)
    {
        var user = await this.userRepository.SelectAsync(u => u.PhoneNumber == PhoneNumber);

        if (user is null)
            throw new CustomException(404, "User not found");

        if (NewPassword != ConfirmPassword)
            throw new CustomException(400, "New password and confirm password aren't equal");

        var hash = PasswordHelper.Hash(NewPassword);

        user.Salt = hash.Salt;
        user.Password = hash.Hash;

        var updated = this.userRepository.Update(user);

        return await this.userRepository.SaveAsync();
    }

    public async Task<UserForResultDto> ModifyAsync(long id, UserForUpdateDto dto)
    {
        var user = await this.userRepository.SelectAsync(u => u.Id == id);
        if (user is null)
            throw new CustomException(404, "User not found");

        if (dto is not null)
        {
            user.FirstName = string.IsNullOrEmpty(dto.FirstName) ? user.FirstName : dto.FirstName;
            user.LastName = string.IsNullOrEmpty(dto.LastName) ? user.LastName : dto.LastName;
            user.PhoneNumber = string.IsNullOrEmpty(dto.PhoneNumber) ? user.PhoneNumber : dto.PhoneNumber;
            user.IsStudyForeign = dto.IsStudyForeign;

            user.UpdatedAt = TimeHelper.GetCurrentServerTime();
            this.userRepository.Update(user);
            var result = await this.userRepository.SaveAsync();
        }
        var person = this.mapper.Map(dto, user);
       /* await this.userRepository.SaveAsync();*/

        return this.mapper.Map<UserForResultDto>(person);
    }

    public async Task<UserForResultDto> ModifyTelegramId(long id, long telegramId)
    {
        var user = await this.userRepository.SelectAll()
            .Where(u => u.Id == id)
            .Include(u => u.UserGroups)
            .FirstOrDefaultAsync();
        if (user is null)
            throw new CustomException(404, "User not found");

        user.TelegramId = telegramId;
        await this.userRepository.SaveAsync();

        return this.mapper.Map<UserForResultDto>(user);
    }

    public async Task<bool> RemoveAsync(long id)
    {
        var user = await userRepository.SelectAsync(u => u.Id == id);
        if (user is null)
            throw new CustomException(404, "User not found");

        await userRepository.DeleteAsync(id);
        var result = await this.userRepository.SaveAsync();
        return result;
    }

    public async Task<IEnumerable<UserForResultDto>> RetrieveAllAsync(PaginationParams @params)
    {
        var users = await this.userRepository.SelectAll()
            .Where(x => x.Role.Equals((UserRole)0))
            .Include(u => u.UserGroups)
            .ThenInclude(ug => ug.Group)
            .AsNoTracking()
            .ToPagedList(@params)
            .ToListAsync();

        var mappedUsers = this.mapper.Map<IEnumerable<UserForResultDto>>(users);

        foreach (var userDto in mappedUsers)
        {
            var user = users.FirstOrDefault(u => u.Id == userDto.Id); // Find the corresponding user
            if (user is not null)
            {
                // Map and populate the Groups property in the DTO
                userDto.Groups = this.mapper.Map<ICollection<GroupForResultDto>>(user.UserGroups.Select(ug => ug.Group));
            }
        }

        return mappedUsers;
    }

    public async Task<UserForResultDto> RetrieveByIdAsync(long id)
    {
        var user = await this.userRepository.SelectAll()
        .Where(u => u.Id == id)
        .Include(u => u.UserGroups)
        .ThenInclude(ug => ug.Group)
        .AsNoTracking()
        .FirstOrDefaultAsync();

        if (user is null)
            throw new CustomException(404, "User Not Found");

        var userDto = this.mapper.Map<UserForResultDto>(user);
        userDto.Groups = this.mapper.Map<ICollection<GroupForResultDto>>(user.UserGroups.Select(ug => ug.Group));

        return userDto;
    }

    public async Task<UserForResultDto> RetrieveByIdWithQuizzesAsync(long id)
    {
        var user = await this.userRepository.SelectAll()
        .Where(u => u.Id == id)
        .Include(u => u.UserGroups)
        .ThenInclude(ug => ug.Group)
        .Include(u => u.QuizResults)
        .ThenInclude(u => u.Quiz)
        .AsNoTracking()
        .FirstOrDefaultAsync();

        if (user is null)
            throw new CustomException(404, "User Not Found");

        var userDto = this.mapper.Map<UserForResultDto>(user);
        userDto.Groups = this.mapper.Map<ICollection<GroupForResultDto>>(user.UserGroups.Select(ug => ug.Group));

        var lastThreeQuizzesDto = user.QuizResults?
            .OrderByDescending(qr => qr.Id)
            .Take(3)
            .Select(qr => this.mapper.Map<QuizForResultDto>(qr.Quiz))
            .ToList();

        userDto.LastQuizzes = lastThreeQuizzesDto;

        return userDto;
    }


    public async Task<UserForResultDto> RetrieveByPhoneNumberAsync(string phoneNumber)
    {
        var user = await this.userRepository.SelectAsync(u => u.PhoneNumber == phoneNumber);
        if (user is null)
            throw new CustomException(404, "User Not Found");

        return this.mapper.Map<UserForResultDto>(user);
    }

    public async Task<bool> SendMessageByTelegramIdAsync(long TelegramId, string text, string url)
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
        var result = await botClient.SendTextMessageAsync(
            chatId: TelegramId,
            text: text,
            replyMarkup: inlineKeyboard);
        
        if (result != null)
        {
            return true;
        }
        return false;
    }

    public async Task<bool> TelegramIdExistsAsync(long TelegramId)
    {
        var user = await this.userRepository.SelectAll()
            .Where(u => u.TelegramId == TelegramId)
            .Include(u => u.UserGroups)
            .FirstOrDefaultAsync();

        if (user is null)
            throw new CustomException(404, "User not found");

        return true;
    }

    public async Task<IEnumerable<UserForResultDto>> SearchAllAsync(string search, PaginationParams @params)
    {
        var users = await this.userRepository.SelectAll()
            .Where(x => x.FirstName.ToLower().Contains(search.ToLower())
            || x.LastName.ToLower().Contains(search.ToLower())
            || x.PhoneNumber.Contains(search))
            .Where(xrole => xrole.Role.Equals((UserRole)0))
            .Include(u => u.UserGroups)
            .ThenInclude(ug => ug.Group)
            .ToPagedList(@params)
            .ToListAsync();

        var mappedUsers = this.mapper.Map<IEnumerable<UserForResultDto>>(users);

        foreach (var userDto in mappedUsers)
        {
            var user = users.FirstOrDefault(u => u.Id == userDto.Id); // Find the corresponding user
            if (user is not null)
            {
                // Map and populate the Groups property in the DTO
                userDto.Groups = this.mapper.Map<ICollection<GroupForResultDto>>(user.UserGroups.Select(ug => ug.Group));
            }
        }

        return mappedUsers;
    }

    public async Task<IEnumerable<UserForResultDto>> RetrieveAllAdminsAsync(PaginationParams @params)
    {
        var users = await this.userRepository.SelectAll()
            .Where(x => x.Role.Equals((UserRole)2)
            || x.Role.Equals((UserRole)3))
            .ToPagedList(@params)
            .ToListAsync();

        var mappedUsers = this.mapper.Map<IEnumerable<UserForResultDto>>(users);

        return mappedUsers;
    }

    public async Task<IEnumerable<UserForResultDto>> SearchAdminsAsync(string search, PaginationParams @params)
    {

        var users = await this.userRepository.SelectAll()
            .Where(x => x.FirstName.ToLower().Contains(search.ToLower())
            || x.LastName.ToLower().Contains(search.ToLower())
            || x.PhoneNumber.Contains(search))
            .Where(x => x.Role.Equals((UserRole)2)
            || x.Role.Equals((UserRole)3))
            .ToPagedList(@params)
            .ToListAsync();

        var mappedUsers = this.mapper.Map<IEnumerable<UserForResultDto>>(users);

        return mappedUsers;
    }

    public async Task<IEnumerable<UserForResultDto>> RetrieveAllTeachersAsync(PaginationParams @params)
    {
        var users = await this.userRepository.SelectAll()
            .Where(x => x.Role.Equals((UserRole)1))
            .Include(u => u.UserGroups)
            .ThenInclude(ug => ug.Group)
            .AsNoTracking()
            .ToPagedList(@params)
            .ToListAsync();

        var mappedUsers = this.mapper.Map<IEnumerable<UserForResultDto>>(users);

        foreach (var userDto in mappedUsers)
        {
            var user = users.FirstOrDefault(u => u.Id == userDto.Id); // Find the corresponding user
            if (user is not null)
            {
                // Map and populate the Groups property in the DTO
                userDto.Groups = this.mapper.Map<ICollection<GroupForResultDto>>(user.UserGroups.Select(ug => ug.Group));
            }
        }

        return mappedUsers;
    }

    public async Task<IEnumerable<UserForResultDto>> SearchTeachersAsync(string search, PaginationParams @params)
    {
        var users = await this.userRepository.SelectAll()
            .Where(x => x.FirstName.ToLower().Contains(search.ToLower())
            || x.LastName.ToLower().Contains(search.ToLower())
            || x.PhoneNumber.Contains(search))
            .Where(xrole => xrole.Role.Equals((UserRole)1))
            .Include(u => u.UserGroups)
            .ThenInclude(ug => ug.Group)
            .ToPagedList(@params)
            .ToListAsync();

        var mappedUsers = this.mapper.Map<IEnumerable<UserForResultDto>>(users);

        foreach (var userDto in mappedUsers)
        {
            var user = users.FirstOrDefault(u => u.Id == userDto.Id); // Find the corresponding user
            if (user is not null)
            {
                // Map and populate the Groups property in the DTO
                userDto.Groups = this.mapper.Map<ICollection<GroupForResultDto>>(user.UserGroups.Select(ug => ug.Group));
            }
        }

        return mappedUsers;
    }

    public async Task<IEnumerable<UserForResultDto>> GetAllByGroupAsync(long id)
    {
        var userGroups =  await userGroupRepository.
            SelectAll(x => x.GroupId == id)
            .AsNoTracking()
            .ToListAsync();

        List<User> users = new List<User>();

        foreach (var usergroup in userGroups)
        {
            var checkingUsers = await userRepository.SelectAsync(x => x.Id == usergroup.UserId);
            users.Add(checkingUsers);
        }

        var mappedUsers = this.mapper.Map<IEnumerable<UserForResultDto>>(users);

        return mappedUsers;
    }

    public async Task<IEnumerable<UserStatisticsForResultDto>> RetrieveAllStudentsWithStatisticsAsync(PaginationParams @params)
    {
        var users = await this.userRepository.SelectAll()
            .Where(u => u.Role.Equals((UserRole)0))
            .Include(u => u.QuizResults)
            .AsNoTracking()
            .ToPagedList(@params)
            .ToListAsync();

        var mappedUsers = this.mapper.Map<IEnumerable<UserStatisticsForResultDto>>(users);

        foreach(var user in mappedUsers)
        {
            user.TestCount = user.QuizResults.Count();
            user.TotalScore = user.TestCount !=0? user.QuizResults.Sum(x => x.Score)/user.TestCount:0;
        }

        return mappedUsers;

    }

    public async Task<IEnumerable<UserStatisticsForResultDto>> RetrieveAllStudentsWithStatisticsAsync()
    {
        var users = await this.userRepository.SelectAll()
           .Where(u => u.Role.Equals((UserRole)0))
           .Include(u => u.QuizResults)
           .AsNoTracking()
           .ToListAsync();

        var mappedUsers = this.mapper.Map<IEnumerable<UserStatisticsForResultDto>>(users);

        foreach (var user in mappedUsers)
        {
            user.TestCount = user.QuizResults.Count();
            user.TotalScore = user.TestCount != 0 ? user.QuizResults.Sum(x => x.Score) / user.TestCount : 0;
        }

        return mappedUsers;
    }
}