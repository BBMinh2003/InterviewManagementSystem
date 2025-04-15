using System;
using AutoMapper;
using IMS.API.ConfigurationOptions.Resolvers;
using IMS.Business.Handlers;
using IMS.Business.Handlers.UserHandlers;
using IMS.Business.ViewModels;
using IMS.Business.ViewModels.Benefit;
using IMS.Business.ViewModels.Level;
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
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy != null ? src.CreatedBy.FullName : "N/A"))
            .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy != null ? src.CreatedBy!.FullName : "N/A"))
            .ForMember(dest => dest.DeletedBy, opt => opt.MapFrom(src => src.DeletedBy != null ? src.CreatedBy!.FullName : "N/A"))
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

        // Map từ Job -> JobViewModel
        CreateMap<Job, JobViewModel>()
            .ForMember(dest => dest.JobSkills, opt => opt.MapFrom(src => src.JobSkills.Select(js => new SkillViewModel
            {
                Id = js.SkillId,
                Name = js.Skill != null ? js.Skill.Name : ""
            }).ToList()))
            .ForMember(dest => dest.JobLevels, opt => opt.MapFrom(src => src.JobLevels.Select(jl => new LevelViewModel
            {
                Id = jl.LevelId,
                Name = jl.Level != null ? jl.Level.Name : ""
            }).ToList()))
            .ForMember(dest => dest.JobBenefits, opt => opt.MapFrom(src => src.JobBenefits.Select(jb => new BenefitViewModel
            {
                Id = jb.BenefitId,
                Name = jb.Benefit != null ? jb.Benefit.Name : ""
            }).ToList()));

        // Map từ JobViewModel -> Job (Không ánh xạ Navigation Property)
        CreateMap<JobViewModel, Job>()
            .ForMember(dest => dest.JobSkills, opt => opt.Ignore())
            .ForMember(dest => dest.JobLevels, opt => opt.Ignore())
            .ForMember(dest => dest.JobBenefits, opt => opt.Ignore());

        // Map từ JobSkill -> SkillViewModel
        CreateMap<JobSkill, SkillViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.SkillId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Skill != null ? src.Skill.Name : ""));

        // Map từ JobLevel -> LevelViewModel
        CreateMap<JobLevel, LevelViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.LevelId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Level != null ? src.Level.Name : ""));

        // Map từ JobBenefit -> BenefitViewModel
        CreateMap<JobBenefit, BenefitViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.BenefitId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Benefit != null ? src.Benefit.Name : ""));

        CreateMap<JobCreateCommand, Job>()
            .ForMember(dest => dest.JobSkills, opt => opt.MapFrom(src => src.JobSkills
            .Select(skillId => new JobSkill { SkillId = skillId })))
            .ForMember(dest => dest.JobLevels, opt => opt.MapFrom(src => src.JobLevels
            .Select(levelId => new JobLevel { LevelId = levelId })))
            .ForMember(dest => dest.JobBenefits, opt => opt.MapFrom(src => src.JobBenefits
            .Select(benefitId => new JobBenefit { BenefitId = benefitId })));

        CreateMap<JobUpdateCommand, Job>()
            .ForMember(dest => dest.JobSkills, opt => opt.MapFrom(src => src.JobSkills
            .Select(skillId => new JobSkill { SkillId = skillId })))
            .ForMember(dest => dest.JobLevels, opt => opt.MapFrom(src => src.JobLevels
            .Select(levelId => new JobLevel { LevelId = levelId })))
            .ForMember(dest => dest.JobBenefits, opt => opt.MapFrom(src => src.JobBenefits
            .Select(benefitId => new JobBenefit { BenefitId = benefitId })));


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

        CreateMap<Offer, OfferViewModel>()
           .ForMember(dest => dest.CandidateName, opt => opt.MapFrom(src => src.Candidate != null ? src.Candidate.Name : " "))
           .ForMember(dest => dest.InterviewInfo, opt => opt.MapFrom(src => src.Interview != null ? src.Interview.Title : " "))
           .ForMember(dest => dest.RecruiterOwnerName, opt => opt.MapFrom(src => src.RecruiterOwner != null ? src.RecruiterOwner.FullName : " "))
           .ForMember(dest => dest.ApproverName, opt => opt.MapFrom(src => src.ApprovedBy!.FullName))
           .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : " "))
           .ForMember(dest => dest.ContactType, opt => opt.MapFrom(src => src.ContactType != null ? src.ContactType.Name : " "))
           .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level != null ? src.Level.Name : " "))
           .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.Position != null ? src.Position.Name : " "))
           .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy != null ? src.UpdatedBy.UserName : " "));

        CreateMap<OfferUpdateCommand, Offer>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ApprovedById, opt => opt.MapFrom(src => src.ApproverId))
            .ForMember(dest => dest.Candidate, opt => opt.Ignore())
            .ForMember(dest => dest.Department, opt => opt.Ignore())
            .ForMember(dest => dest.RecruiterOwner, opt => opt.Ignore())
            .ForMember(dest => dest.ApprovedBy, opt => opt.Ignore())
            .ForMember(dest => dest.Position, opt => opt.Ignore())
            .ForMember(dest => dest.ContactType, opt => opt.Ignore())
            .ForMember(dest => dest.Level, opt => opt.Ignore())
            .ForMember(dest => dest.Interview, opt => opt.Ignore());

        CreateMap<User, GetUserViewModel>()
          .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
          .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
          .ForMember(dest => dest.Roles, opt => opt.MapFrom<GetUserRolesResolver>());

    }
}

