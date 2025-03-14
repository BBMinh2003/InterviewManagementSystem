using System;
using AutoMapper;
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

    }
}

internal class UserViewModel
{
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public string? DeletedBy { get; set; }

}