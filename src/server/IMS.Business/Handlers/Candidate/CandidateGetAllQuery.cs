using IMS.Business.ViewModels;
using MediatR;

namespace IMS.Business.Handlers;

public class CandidateGetAllQuery : IRequest<IEnumerable<CandidateViewModel>>
{

}
