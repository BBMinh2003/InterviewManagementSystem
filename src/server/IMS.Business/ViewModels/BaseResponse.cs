using System;

namespace IMS.Business.ViewModels;

public class BaseResponse
{
    public bool Success { get; set; } = true;
    public string? Message { get; set; }
}
