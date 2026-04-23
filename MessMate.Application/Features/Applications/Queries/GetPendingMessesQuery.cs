using MediatR;
using MessMate.Application.Features.Applications.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Applications.Queries
{
    public record GetPendingMessesQuery() : IRequest<List<PendingMessDto>>;
}
