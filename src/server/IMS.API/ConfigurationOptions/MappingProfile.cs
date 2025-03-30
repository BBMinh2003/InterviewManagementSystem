using System;
using AutoMapper;
using IMS.API.ConfigurationOptions.Resolvers;
using IMS.Business.Handlers;
using IMS.Business.Handlers.UserHandlers;
using IMS.Business.ViewModels;
using IMS.Business.ViewModels.UserViews;
using IMS.Models.Common;
using IMS.Models.Security;

namespace IMS.API.ConfigurationOptions;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User
        CreateMap<User, UserViewModel>()
            .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : ""))
            .ForMember(dest => dest.Roles, opt => opt.MapFrom<UserRolesResolver>());
        CreateMap<UserCreateCommand, User>();

        // Candidate
        CreateMap<Candidate, CandidateViewModel>()
            .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.Position != null ? src.Position.Name : ""))
            .ForMember(dest => dest.RecruiterOwnerName, opt => opt.MapFrom(src => src.RecruiterOwner != null ? src.RecruiterOwner.FullName : ""))
            .ForMember(dest => dest.CandidateSkills, opt => opt.MapFrom(src => src.CandidateSkills.Select(cs => new SkillViewModel
            {
                Id = cs.SkillId,
                Name = cs.Skill != null ? cs.Skill.Name : ""
            }).ToList()));

        CreateMap<CandidateSkill, SkillViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.SkillId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Skill != null ? src.Skill.Name : ""));

        // Map từ CandidateViewModel sang Candidate
        CreateMap<CandidateViewModel, Candidate>()
            .ForMember(dest => dest.Position, opt => opt.Ignore()) // Không tự động ánh xạ Navigation Property
            .ForMember(dest => dest.RecruiterOwner, opt => opt.Ignore())
            .ForMember(dest => dest.CandidateSkills, opt => opt.Ignore()); // Sẽ xử lý riêng phần kỹ năng

        // Mapping từ CandidateCreateUpdateCommand -> Candidate
        CreateMap<CandidateUpdateCommand, Candidate>()
            .ForMember(dest => dest.CandidateSkills, opt => opt.MapFrom(src => src.CandidateSkillIds
                .Select(skillId => new CandidateSkill { SkillId = skillId })));

        CreateMap<TimeOnly, TimeOnly>().ConvertUsing(t => t);
        CreateMap<DateOnly, DateOnly>().ConvertUsing(d => d);



        CreateMap<Interview, InterviewViewModel>()
           .ForMember(dest => dest.CandidateName, opt => opt.MapFrom(src => src.Candidate != null ? src.Candidate.Name : " "))
           .ForMember(dest => dest.JobName, opt => opt.MapFrom(src => src.Job != null ? src.Job.Title : " "))
           .ForMember(dest => dest.RecruiterOwnerName, opt => opt.MapFrom(src => src.RecruiterOwner != null ? src.RecruiterOwner.FullName : " "))
           .ForMember(dest => dest.Interviewers, opt => opt.MapFrom(src => src.Interviewers));

        CreateMap<IntervewerInterview, InterviewerInterviewViewModel>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User != null ? src.User.FullName : " "));

        CreateMap<InterviewCreateCommand, Interview>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Interviewers, opt => opt.Ignore());

        CreateMap<InterviewUpdateCommand, Interview>()
           .ForMember(dest => dest.Interviewers, opt => opt.Ignore());
    }
}

