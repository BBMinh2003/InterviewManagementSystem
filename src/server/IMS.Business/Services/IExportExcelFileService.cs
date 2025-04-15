using System;

namespace IMS.Business.Services;

public interface IExportExcelFileService
{
    Task<byte[]> ExportOffersAsync(DateTime fromDate, DateTime toDate);
}