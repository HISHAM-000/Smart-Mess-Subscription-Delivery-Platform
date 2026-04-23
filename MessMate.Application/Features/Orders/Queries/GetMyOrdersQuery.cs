using MediatR;
using MessMate.Application.Features.Orders.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Orders.Queries
{
    public record GetMyOrdersQuery() : IRequest<List<OrderDto>>;
}
