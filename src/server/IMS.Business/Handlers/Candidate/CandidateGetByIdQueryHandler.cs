using AutoMapper;
using IMS.Business.Services;
using IMS.Business.ViewModels;
using IMS.Core.Exceptions;
using IMS.Data.UnitOfWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Handlers;

public class CandidateGetByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, AzureStorageService azureStorageService) :
    BaseHandler(unitOfWork, mapper),
    IRequestHandler<CandidateGetByIdQuery, CandidateViewModel>
{
    private readonly AzureStorageService _azureStorageService = azureStorageService;

    public async Task<CandidateViewModel> Handle(
        CandidateGetByIdQuery request,
        CancellationToken cancellationToken)
    {
        var candidate = await _unitOfWork.CandidateRepository.GetQuery()
            .Include(c => c.RecruiterOwner)
            .Include(c => c.Position)
            .Include(c => c.CandidateSkills).ThenInclude(cs => cs.Skill)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken) ??
            throw new ResourceNotFoundException("Candidate not found");

        var candidateViewModel = _mapper.Map<CandidateViewModel>(candidate);
        
        // Fetch file from Azure Storage
        if (!string.IsNullOrEmpty(candidate.CV_Attachment))
        {
            // using var fileStream = await _azureStorageService.DownloadFileFromUrlAsync("https://meomeo.blob.core.windows.net/ims-files/" + candidate.CV_Attachment);
            // using var memoryStream = new MemoryStream();
            // await fileStream.CopyToAsync(memoryStream);
            // byte[] fileBytes = memoryStream.ToArray();
            // candidateViewModel.CvFile = Convert.ToBase64String(fileBytes);

            candidateViewModel.CvFile =  _azureStorageService.GetFileUrl(candidate.CV_Attachment);
        }

        return candidateViewModel;
    }
}  