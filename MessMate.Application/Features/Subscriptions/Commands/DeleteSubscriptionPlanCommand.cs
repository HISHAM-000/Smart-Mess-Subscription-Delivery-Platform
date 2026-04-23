using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Subscriptions.Commands
{
    public record DeleteSubscriptionPlanCommand(int PlanId) : IRequest<bool>;
}
