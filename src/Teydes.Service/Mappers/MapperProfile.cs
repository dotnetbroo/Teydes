using AutoMapper;
using Teydes.Service.DTOs.Users;
using Teydes.Service.DTOs.Groups;
using Teydes.Service.DTOs.Courses;
using Teydes.Service.DTOs.Quizzes;
using Teydes.Service.DTOs.Answers;
using Teydes.Domain.Entities.Users;
using Teydes.Domain.Entities.Quizes;
using Teydes.Service.DTOs.Questions;
using Teydes.Domain.Entities.Courses;
using Teydes.Service.DTOs.UserGroups;
using Teydes.Service.DTOs.Submissions;
using Teydes.Service.DTOs.QuizResults;

namespace Teydes.Service.Mappers;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        // User
        CreateMap<User, UserForResultDto>().ReverseMap();
        CreateMap<User, UserForUpdateDto>().ReverseMap();
        CreateMap<User, UserForCreationDto>().ReverseMap();
        CreateMap<User, UserForChangeRoleDto>().ReverseMap();
        CreateMap<User, UserForGroupResultDto>().ReverseMap();
        CreateMap<User, UserForChangePasswordDto>().ReverseMap();
        CreateMap<User, UserStatisticsForResultDto>().ReverseMap();
        // Course
        CreateMap<Course, CourseForUpdateDto>().ReverseMap();
        CreateMap<Course, CourseForResultDto>().ReverseMap();
        CreateMap<Course, CourseForCreationDto>().ReverseMap();

        // Group
        CreateMap<Group, GroupForResultDto>().ReverseMap();
        CreateMap<Group, GroupForUpdateDto>().ReverseMap();
        CreateMap<Group, GroupForCreationDto>().ReverseMap();
        CreateMap<Group, GroupForStudentRankingResultDto>().ReverseMap();
        

        // UserGroup 
        CreateMap<UserGroup, UserGroupForResultDto>().ReverseMap();
        CreateMap<UserGroup, UserGroupForUpdateDto>().ReverseMap();
        CreateMap<UserGroup, UserGroupForCreationDto>().ReverseMap();

        // Quiz
        CreateMap<Quiz, QuizForResultDto>().ReverseMap();
        CreateMap<Quiz, QuizForUpdateDto>().ReverseMap();
        CreateMap<Quiz, QuizForCreationDto>().ReverseMap();

        // Question
        CreateMap<Question, QuestionForResultDto>().ReverseMap();
        CreateMap<Question, QuestionForUpdateDto>().ReverseMap();
        CreateMap<Question, QuestionForCreationDto>().ReverseMap();
        CreateMap<Question, QuestionForQuizResultDto>().ReverseMap();

        // Answers
        CreateMap<QuestionAnswer, QuestionAnswerForUpdateDto>().ReverseMap();
        CreateMap<QuestionAnswer, QuestionAnswerForResultDto>().ReverseMap();
        CreateMap<QuestionAnswer, QuestionAnswerForCreationDto>().ReverseMap();

        // Submission
        CreateMap<Submission, SubmissionForResultDto>().ReverseMap();
        CreateMap<Submission, SubmissionForUpdateDto>().ReverseMap();
        CreateMap<Submission, SubmissionForCreationDto>().ReverseMap();
        CreateMap<Submission, SubmissionForQuizResultDto>().ReverseMap();
        // QuizResult
        CreateMap<QuizResult, QuizResultForResultDto>().ReverseMap();

    }
}
