using AutoMapper;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using MediatR;

namespace IMS.Business.Handlers;

public class JobDeleteByIdCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) :
    BaseHandler(unitOfWork, mapper),
    IRequestHandler<JobDeleteByIdCommand, bool>
{
    public async Task<bool> Handle(
        JobDeleteByIdCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.JobRepository.GetByIdAsync(request.Id) ??
            throw new ResourceNotFoundException($"Job with {request.Id} is not found");

        _unitOfWork.JobRepository.Delete(entity);
        return await _unitOfWork.SaveChangesAsync() > 0;
    }
}
