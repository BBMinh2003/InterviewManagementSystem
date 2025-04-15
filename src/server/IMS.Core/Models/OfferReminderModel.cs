using System;

namespace IMS.Core.Models;

public class OfferReminderModel
{
    public string CandidateName { get; set; } = string.Empty;

    public string PositionName { get; set; } = string.Empty;

    public DateTime DueDate { get; set; } 

    public string OfferLink { get; set; } = string.Empty;

    public string RecruiterEmail { get; set; } = string.Empty;

}