using IMS.Business.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace IMS.Business.Handlers.Job;

public class JobImportExcelCommand : IRequest<IEnumerable<JobViewModel>>
{
    public required IFormFile ExcelFile { get; set; }
}
