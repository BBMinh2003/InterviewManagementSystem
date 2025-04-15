using AutoMapper;
using IMS.Business.ViewModels.Common;
using IMS.Data.UnitOfWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers.Dashboard;

public class DashboardGetAllQueryHandler : BaseHandler, IRequestHandler<DashboardGetAllQuery, DashboardViewModel>
{
    public DashboardGetAllQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    public async Task<DashboardViewModel> Handle(DashboardGetAllQuery request, CancellationToken cancellationToken)
    {
        return new DashboardViewModel
        {
            InterviewCount = await _unitOfWork.InterviewRepository.GetQuery().CountAsync(),
            JobCount = await _unitOfWork.JobRepository.GetQuery().CountAsync(),
            CandidateCount = await _unitOfWork.CandidateRepository.GetQuery().CountAsync(),
            OfferCount = await _unitOfWork.InterviewRepository.GetQuery().CountAsync(),
        };
    }
}
