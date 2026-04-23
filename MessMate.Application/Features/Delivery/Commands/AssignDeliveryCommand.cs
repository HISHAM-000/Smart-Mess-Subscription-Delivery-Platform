using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Delivery.Commands
{
    public record AssignDeliveryCommand(
        int OrderId,
        int StaffId
    ) : IRequest<AssignDeliveryResult>;

    public record AssignDeliveryResult(
        int DeliveryId,
        string OTP,
        string Message
    );
}
