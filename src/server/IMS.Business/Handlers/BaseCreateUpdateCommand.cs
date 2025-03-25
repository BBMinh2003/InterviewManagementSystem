using MediatR;

namespace IMS.Business.Handlers;

public class BaseCreateUpdateCommand<T>: IRequest<T>
{
    public Guid? Id { get; set; }
}
