using System;

namespace IMS.Core.Models;

public class JobOfferEmailModel
{
    public string CandidateName { get; set; } = string.Empty;
    public string CandidatePosition { get; set; } = string.Empty;
    public string OfferDueDate { get; set; } = string.Empty;
    public string OfferDetailUrl { get; set; } = string.Empty;
    public string RecruiterEmail { get; set; } = string.Empty;
    public string AttachmentPath { get; set; } = string.Empty;
}
