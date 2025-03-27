using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Core.Enums;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using IMS.Models.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class InterviewCreateCommand : BaseCreateCommand<InterviewViewModel>
{
    [Required(ErrorMessage = "The {0} field is required")]
    public required Guid CandidateId { get; set; }

    [Required(ErrorMessage = "The {0} field is required")]
    public required Guid JobId { get; set; }

    [Required(ErrorMessage = "The {0} field is required")]
    public required Guid RecruiterOwnerId { get; set; }

    [Required(ErrorMessage = "The {0} field is required")]
    [StringLength(255, ErrorMessage = "The {0} field must be a string with a maximum length of {1}")]
    public required string Title { get; set; }

    [StringLength(255, ErrorMessage = "The {0} field must be a string with a maximum length of {1}")]
    public string? Note { get; set; }

    [StringLength(255, ErrorMessage = "The {0} field must be a string with a maximum length of {1}")]
    public string? Location { get; set; }

    [StringLength(255, ErrorMessage = "The {0} field must be a string with a maximum length of {1}")]
    public string? MeetingUrl { get; set; }

    [Required(ErrorMessage = "The {0} field is required")]
    public Result Result { get; set; } = Result.NotApplicable;

    public InterviewStatus Status { get; set; } = InterviewStatus.New;

    [Required(ErrorMessage = "The {0} field is required")]
    public TimeOnly StartAt { get; set; }

    [Required(ErrorMessage = "The {0} field is required")]
    public TimeOnly EndAt { get; set; }

    [Required(ErrorMessage = "The {0} field is required")]
    public DateOnly InterviewDate { get; set; }

    public List<Guid> InterviewerIds { get; set; } = [];

}

public class InterviewCreateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) :
    BaseHandler(unitOfWork, mapper),
    IRequestHandler<InterviewCreateCommand, InterviewViewModel>
{
    public async Task<InterviewViewModel> Handle(InterviewCreateCommand request, CancellationToken cancellationToken)
    {
        var existedInterview = await _unitOfWork.InterviewRepository.GetQuery()
    .FirstOrDefaultAsync(x => x.CandidateId == request.CandidateId && x.JobId == request.JobId && x.StartAt == request.StartAt);

        if (existedInterview != null)
        {
            throw new ResourceUniqueException($"An interview for this candidate and job at the specified time already exists.");
        }

        var entity = new Interview
        {
            CandidateId = request.CandidateId,
            JobId = request.JobId,
            RecruiterOwnerId = request.RecruiterOwnerId,
            Title = request.Title,
            Note = request.Note,
            Location = request.Location,
            MeetingUrl = request.MeetingUrl,
            Result = request.Result,
            Status = request.Status,
            StartAt = request.StartAt,
            EndAt = request.EndAt,
            InterviewDate = request.InterviewDate
        };

        _unitOfWork.InterviewRepository.Add(entity);
        var result = await _unitOfWork.SaveChangesAsync();

        if (result == 0)
        {
            throw new DatabaseBadRequestException("Failed to save interview.");
        }

        entity.Interviewers = request.InterviewerIds
            .Select(interviewerId => new IntervewerInterview { InterviewId = entity.Id, UserId = interviewerId })
            .ToList();

        await _unitOfWork.SaveChangesAsync();

        var createdEntity = await _unitOfWork.InterviewRepository.GetQuery()
            .Include(x => x.Candidate)
            .Include(x => x.Job)
            .Include(x => x.RecruiterOwner)
            .Include(x => x.Interviewers)
            .ThenInclude(ii => ii.User)
            .FirstOrDefaultAsync(x => x.Id == entity.Id)
            ?? throw new ResourceNotFoundException($"Interview with {entity.Id} is not found");

        return _mapper.Map<InterviewViewModel>(createdEntity);
    }
}
