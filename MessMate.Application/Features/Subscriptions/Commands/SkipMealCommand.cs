using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Subscriptions.Commands
{
    public record SkipMealCommand(int OrderId) : IRequest<SkipMealResult>;
    public record SkipMealResult(int SkipId, decimal RefundAmount, string Message);
}
