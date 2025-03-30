using AutoMapper;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using MediatR;

namespace IMS.Business.Handlers;

public class InterviewDeleteByIdCommandHandler : BaseHandler,
    IRequestHandler<InterviewDeleteByIdCommand, bool>
{
    public InterviewDeleteByIdCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    public async Task<bool> Handle(InterviewDeleteByIdCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unitOfWork.InterviewRepository.GetByIdAsync(request.Id) ??
            throw new ResourceNotFoundException($"Interivew with {request.Id} is not found");

        _unitOfWork.InterviewRepository.Delete(entity);
        return await _unitOfWork.SaveChangesAsync() > 0;
    }
}
