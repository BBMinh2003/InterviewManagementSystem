using System;
using System.ComponentModel.DataAnnotations;

namespace IMS.Business.Handlers;

public class OfferExportQuery
{
    [Required]
    public DateTime FromDate { get; set; }

    [Required]
    public DateTime ToDate { get; set; }

}
