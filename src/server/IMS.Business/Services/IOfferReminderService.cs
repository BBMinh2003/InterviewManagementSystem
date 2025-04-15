namespace IMS.Business.Services;

public interface IOfferReminderService
{
    Task CheckAndSendApprovalRemindersAsync();
}
