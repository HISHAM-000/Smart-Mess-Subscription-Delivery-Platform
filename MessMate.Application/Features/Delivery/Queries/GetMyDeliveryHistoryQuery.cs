using MediatR;
using MessMate.Application.Features.Delivery.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Delivery.Queries
{
    public record GetMyDeliveryHistoryQuery() : IRequest<List<DeliveryHistoryDto>>;
}
