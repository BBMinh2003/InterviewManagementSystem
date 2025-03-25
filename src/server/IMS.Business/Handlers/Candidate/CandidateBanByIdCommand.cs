using System.ComponentModel.DataAnnotations;
using MediatR;

namespace IMS.Business.Handlers;

public class CandidateBanByIdCommand : IRequest<bool>
{
    [Required(ErrorMessage = "The {0} field is required")]
    public required Guid Id { get; set; }
}
