using System;
using IMS.Models.Common;

namespace IMS.Business.ViewModels;

public class OfferViewModel
{
    public required Guid Id {get; set;}
    public required string CandidateName { get; set; }

    public required string DepartmentName { get; set; }

    public required string RecruiterOwnerName { get; set; }

    public required string ApproverName { get; set; }

    public required string PositionName { get; set; }

    public string? InterviewInfo { get; set; }

    public string? InterviewNote { get; set; }
    
    public required string ContactType { get; set; }

    public required string Level { get; set; }

    public required decimal BasicSalary { get; set; }

    public string? Note { get; set; } = "N/A";

    public DateTime DueDate { get; set; }

    public DateTime ContactPeriodFrom { get; set; }

    public DateTime ContactPeriodTo { get; set; }

    public OfferStatus Status { get; set; }

}
