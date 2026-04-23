using MessMate.Application.Features.Orders.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Orders.Queries
{
    public record GetMessOrdersQuery(string? Date) : IRequest<List<MessOrderDto>>;
}
