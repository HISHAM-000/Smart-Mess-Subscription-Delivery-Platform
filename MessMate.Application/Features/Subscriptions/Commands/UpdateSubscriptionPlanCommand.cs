using MediatR;
using MessMate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Subscriptions.Commands
{
    public record UpdateSubscriptionPlanCommand : IRequest<bool>
    {
        [JsonIgnore]
        public int PlanId { get; init; }
        public string Name { get; init; } = null!;
        public PlanType PlanType { get; init; }
        public decimal Price { get; init; }
        public int MinActiveDays { get; init; }

        public bool IsBreakfast { get; init; }
        public bool IsLunch { get; init; }
        public bool IsDinner { get; init; }
    }

}
