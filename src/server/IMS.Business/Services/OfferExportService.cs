using System;
using ClosedXML.Excel;
using IMS.Data.UnitOfWorks;
using Microsoft.EntityFrameworkCore;

namespace IMS.Business.Services;

public class OfferExportService : IExportExcelFileService
{
     private readonly IUnitOfWork _unitOfWork;

    public OfferExportService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<byte[]> ExportOffersAsync(DateTime fromDate, DateTime toDate)
    {
        var offers = await _unitOfWork.OfferRepository.GetQuery()
            .Include(o => o.Candidate)
            .Include(o => o.ContactType)
            .Include(o => o.ApprovedBy)
            .Include(o => o.Department)
            .Include(o => o.Level)
            .Include(o => o.Position)
            .Where(o => o.CreatedAt >= fromDate && o.CreatedAt <= toDate)
            .ToListAsync();

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Offers");

        var headers = new string[] 
        { 
            "STT", 
            "Candidate Name", 
            "Email", 
            "Approver", 
            "Department", 
            "Notes", 
            "Status"
        };

        for (int i = 0; i < headers.Length; i++)
        {
            worksheet.Cell(1, i + 1).Value = headers[i];
            worksheet.Cell(1, i + 1).Style.Font.Bold = true;
        }

        int row = 2;
        foreach (var offer in offers)
        {
            worksheet.Cell(row, 1).Value = row - 1; 
            worksheet.Cell(row, 2).Value = offer.Candidate?.Name;
            worksheet.Cell(row, 3).Value = offer.Candidate?.Email;
            worksheet.Cell(row, 4).Value = offer.ApprovedBy?.FullName;
            worksheet.Cell(row, 5).Value = offer.Department?.Name;
            worksheet.Cell(row, 6).Value = offer.Note;
            worksheet.Cell(row, 7).Value = offer.Status.ToString();
            
            row++;
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}