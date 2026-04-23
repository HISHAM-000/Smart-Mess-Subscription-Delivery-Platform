using MediatR;
using MessMate.Application.Features.Subscriptions.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Subscriptions.Queries
{
    public record GetPlansByMessQuery(int MessId) : IRequest<List<SubscriptionPlanDto>>;
}
