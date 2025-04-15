using AutoMapper;
using IMS.Business.ViewModels;
using IMS.Data.UnitOfWorks;
using IMS.Models.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace IMS.Business.Handlers.Job;

public class JobImportExcelCommandHandler :
    BaseHandler,
    IRequestHandler<JobImportExcelCommand, IEnumerable<JobViewModel>>
{
    public JobImportExcelCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    {
    }

    public async Task<IEnumerable<JobViewModel>> Handle(JobImportExcelCommand request, CancellationToken cancellationToken)
    {
        using (var stream = new MemoryStream())
        {
            await request.ExcelFile.CopyToAsync(stream, cancellationToken);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets[0];
                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    var title = worksheet.Cells[row, 1].Text?.Trim();

                    if (string.IsNullOrWhiteSpace(title))
                        break;

                    var jobCommand = new JobCreateCommand
                    {
                        Title = title,
                        WorkingAddress = worksheet.Cells[row, 2].Text,
                        MinSalary = decimal.TryParse(worksheet.Cells[row, 3].Text, out var minSalary) ? minSalary : 0,
                        MaxSalary = decimal.TryParse(worksheet.Cells[row, 4].Text, out var maxSalary) ? maxSalary : 0,
                        Description = worksheet.Cells[row, 5].Text,
                        StartDate = DateTime.TryParse(worksheet.Cells[row, 6].Text, out var startDate) ? startDate : DateTime.MinValue,
                        EndDate = DateTime.TryParse(worksheet.Cells[row, 7].Text, out var endDate) ? endDate : DateTime.MinValue,
                        JobSkills = ConvertToGuidList(worksheet.Cells[row, 8].Text, LookupType.Skill),
                        JobBenefits = ConvertToGuidList(worksheet.Cells[row, 9].Text, LookupType.Benefit),
                        JobLevels = ConvertToGuidList(worksheet.Cells[row, 10].Text, LookupType.Level)
                    };
                    jobCommand.Status = JobStatus.Draft;
                    var entity = _mapper.Map<Models.Common.Job>(jobCommand);
                    _unitOfWork.JobRepository.Add(entity);
                }
            }
        }

        await _unitOfWork.SaveChangesAsync();

        var jobs = await _unitOfWork.JobRepository.GetQuery()
            .Include(x => x.JobSkills).ThenInclude(js => js.Skill)
            .Include(x => x.JobLevels).ThenInclude(jl => jl.Level)
            .Include(x => x.JobBenefits).ThenInclude(jb => jb.Benefit)
            .ToListAsync(cancellationToken);

        return _mapper.Map<IEnumerable<JobViewModel>>(jobs);
    }

    private List<Guid> ConvertToGuidList(string text, LookupType type)
    {
        if (string.IsNullOrWhiteSpace(text))
            return new List<Guid>();

        var names = text.Split(", ", StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => s.Trim())
                        .ToList();

        IEnumerable<(string Name, Guid Id)> allItems = type switch
        {
            LookupType.Skill => _unitOfWork.GenericSkillRepository
                                           .GetQuery()
                                           .Select(s => new { s.Name, s.Id })
                                           .AsEnumerable()
                                           .Select(x => (x.Name, x.Id)),

            LookupType.Benefit => _unitOfWork.BenefitRepository
                                             .GetQuery()
                                             .Select(b => new { b.Name, b.Id })
                                             .AsEnumerable()
                                             .Select(x => (x.Name, x.Id)),

            LookupType.Level => _unitOfWork.LevelRepository
                                           .GetQuery()
                                           .Select(l => new { l.Name, l.Id })
                                           .AsEnumerable()
                                           .Select(x => (x.Name, x.Id)),

            _ => Enumerable.Empty<(string, Guid)>()
        };

        return allItems
            .Where(item => names.Contains(item.Name, StringComparer.OrdinalIgnoreCase))
            .Select(item => item.Id)
            .ToList();
    }

    private enum LookupType
    {
        Skill,
        Benefit,
        Level
    }
}

