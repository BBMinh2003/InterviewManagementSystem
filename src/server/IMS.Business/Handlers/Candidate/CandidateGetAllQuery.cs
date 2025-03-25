using IMS.Business.ViewModels;
using MediatR;

namespace IMS.Business.Handlers;

// Query để lấy tất cả ứng viên
public class CandidateGetAllQuery : BaseGetAllQuery<CandidateViewModel>
{
}
