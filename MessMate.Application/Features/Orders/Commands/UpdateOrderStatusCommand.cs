using MediatR;
using MessMate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Orders.Commands
{
    public record UpdateOrderStatusCommand(
        int OrderId,
        OrderStatus NewStatus
    ) : IRequest<bool>;
}
