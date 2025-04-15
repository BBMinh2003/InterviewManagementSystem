using System;
using IMS.Core.Models;
using IMS.Data.UnitOfWorks;
using IMS.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IMS.Business.Services;

public class OfferReminderService : IOfferReminderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;
    private readonly ILogger<OfferReminderService> _logger;

    public OfferReminderService(
        IUnitOfWork unitOfWork,
        IEmailService emailService,
        ILogger<OfferReminderService> logger)
    {
        _unitOfWork = unitOfWork;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task CheckAndSendApprovalRemindersAsync()
    {
        var offers = await _unitOfWork.OfferRepository.GetQuery()
            .Include(o => o.Candidate)
            .Include(o => o.Position)
            .Include(o => o.RecruiterOwner)
            .Include(o => o.ApprovedBy)
            .Where(o => o.Status == OfferStatus.WaitingForApproval &&
                       o.DueDate >= DateTime.Today)
            .ToListAsync();

        foreach (var offer in offers)
        {
            try
            {
                if (offer.ApprovedBy?.Email == null)
                {
                    _logger.LogWarning($"Offer {offer.Id} has no approver email");
                    continue;
                }

                var model = new OfferReminderModel
                {
                    CandidateName = offer.Candidate?.Name ?? "N/A",
                    PositionName = offer.Position?.Name ?? "N/A",
                    DueDate = offer.DueDate,
                    OfferLink = $"http://localhost:5108/api/Offer/{offer.Id}",
                    RecruiterEmail = offer.RecruiterOwner?.Email ?? "no-email-provided"
                };


                await _emailService.SendEmailWithTemplateAsync(
                    offer.ApprovedBy.Email,
                    "no-reply-email-IMS-system <Take action on Job offer>",
                    "OfferApprovalReminder",
                    model
                );


                _logger.LogInformation($"Sent reminder for offer {offer.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send reminder for offer {offer.Id}");
            }
        }
    }
}