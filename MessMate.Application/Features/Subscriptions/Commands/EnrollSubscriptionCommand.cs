using MediatR;
using MessMate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Subscriptions.Commands
{
    public record EnrollSubscriptionCommand(
        int PlanId,
        int MessId,
        string DeliveryAddress,
        double DeliveryLat,
        double DeliveryLng,
        PaymentMethod PaymentMethod
    ) : IRequest<int>;
}
