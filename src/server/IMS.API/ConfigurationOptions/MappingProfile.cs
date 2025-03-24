using System;
using AutoMapper;
using IMS.Business.Handlers;
using IMS.Business.ViewModels;
using IMS.Models.Common;
using IMS.Models.Security;

namespace IMS.API.ConfigurationOptions;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User
        CreateMap<UserViewModel, User>().ReverseMap()
            .ForMember(x => x.CreatedBy, opt => opt.MapFrom(e => e.CreatedBy != null ? e.CreatedBy.FullName : string.Empty))
            .ForMember(x => x.UpdatedBy, opt => opt.MapFrom(e => e.UpdatedBy != null ? e.UpdatedBy.FullName : string.Empty))
            .ForMember(x => x.DeletedBy, opt => opt.MapFrom(e => e.DeletedBy != null ? e.DeletedBy.FullName : string.Empty));

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

        // Mapping từ CandidateCreateUpdateCommand -> Candidate
        CreateMap<CandidateCreateUpdateCommand, Candidate>()
            .ForMember(dest => dest.CandidateSkills, opt => opt.MapFrom(src => src.CandidateSkillIds
                .Select(skillId => new CandidateSkill { SkillId = skillId })));
    }
}

internal class UserViewModel
{
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public string? DeletedBy { get; set; }

}