using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Subscriptions.Commands
{
    public record PauseSubscriptionCommand(
        int SubscriptionId,
        DateOnly PauseFrom,
        DateOnly PauseUntil
    ) : IRequest<bool>;
}
