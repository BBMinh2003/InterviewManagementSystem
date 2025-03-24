using MediatR;

namespace IMS.Business.Handlers;

public class BaseCreateCommand<T>: IRequest<T>
{
    public Guid? Id { get; set; }
}
