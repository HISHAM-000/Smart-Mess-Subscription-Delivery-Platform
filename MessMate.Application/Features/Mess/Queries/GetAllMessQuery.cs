using MediatR;
using MessMate.Application.Features.Mess.DTOs;
using MessMate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Mess.Queries
{
    public record GetAllMessesQuery() : IRequest<List<GetAllResponseDto>>;
    
}
