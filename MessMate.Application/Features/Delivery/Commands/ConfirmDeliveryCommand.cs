using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Delivery.Commands
{
    public record ConfirmDeliveryCommand(
        int OrderId,
        string OTP
    ) : IRequest<bool>;
}
