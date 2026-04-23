using MediatR;
using MessMate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Subscriptions.Commands
{
    public record CreateSubscriptionPlanCommand(
    string Name,
    PlanType PlanType,
    decimal Price,
    int MinActiveDays,
    bool IsBreakfast,
    bool IsLunch,
    bool IsDinner
) : IRequest<int>;
}
