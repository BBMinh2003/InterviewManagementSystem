using System;
using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using IMS.Models.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class OfferCreateCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) :
    BaseHandler(unitOfWork, mapper),
    IRequestHandler<OfferCreateCommand, OfferViewModel>
{
    public async Task<OfferViewModel> Handle(OfferCreateCommand request, CancellationToken cancellationToken)
    {
        var existedOffer = await _unitOfWork.OfferRepository.GetQuery()
    .FirstOrDefaultAsync(x => x.CandidateId == request.CandidateId && x.ApprovedById == request.ApproverId && x.RecruiterOwnerId == request.RecruiterOwnerId && x.InterviewId == request.InterviewId);

        if (existedOffer != null)
        {
            throw new ResourceUniqueException($"An offer for this candidate already exists.");
        }

        var entity = new Offer
        {
            CandidateId = request.CandidateId,
            PositionId = request.PositionId,
            RecruiterOwnerId = request.RecruiterOwnerId,
            DepartmentId = request.DepartmentId,
            ApprovedById = request.ApproverId,
            Note = request.Note,
            ContactTypeId = request.ContactTypeId,
            InterviewId = request.InterviewId,
            LevelId = request.LevelId,
            Status = request.Status,
            BasicSalary = request.BasicSalary,
            DueDate = request.DueDate,
            ContactPeriodFrom = request.ContactPeriodFrom,
            ContactPeriodTo = request.ContactPeriodTo,
        };

        _unitOfWork.OfferRepository.Add(entity);
        var result = await _unitOfWork.SaveChangesAsync();

        if (result == 0)
        {
            throw new DatabaseBadRequestException("Failed to save offer.");
        }

        await _unitOfWork.SaveChangesAsync();

        var createdEntity = await _unitOfWork.OfferRepository.GetQuery()
            .Include(x => x.Candidate)
            .Include(x => x.ContactType)
            .Include(x => x.ApprovedBy)
            .Include(x => x.RecruiterOwner)
            .Include(x => x.Department)
            .Include(x => x.Interview)
            .Include(x => x.Level)
            .Include(x => x.Position)
            .FirstOrDefaultAsync(x => x.Id == entity.Id)
            ?? throw new ResourceNotFoundException($"Offer with {entity.Id} is not found");

        return _mapper.Map<OfferViewModel>(createdEntity);
    }
}
