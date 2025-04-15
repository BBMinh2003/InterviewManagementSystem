using IMS.Core.Enums;
using IMS.Models.Security;
using Microsoft.AspNetCore.Http;

namespace IMS.Business.ViewModels;

public class CandidateViewModel : BaseInfoViewModel
{
    public required string Name { get; set; }

    public required string Email { get; set; }

    public required DateTime DateOfBirth { get; set; }

    public required string Address { get; set; }

    public required string Phone { get; set; }

    public required Gender Gender { get; set; }

    public required string  CV_Attachment { get; set; }

    public string? Note { get; set; }

    public CandidateStatus Status { get; set; }

    public int YearOfExperience { get; set; }

    public required HighestLevel HighestLevel { get; set; }

    public Guid PositionId { get; set; }

    public string? PositionName { get; set; }

    public Guid RecruiterOwnerId { get; set; }

    public string? RecruiterOwnerName { get; set; }

    public List<SkillViewModel> CandidateSkills { get; set; } = [];

    public string? CvFile { get; set; }
}
