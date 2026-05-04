using MediatR;
using MessMate.Application.Features.Menu.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Menu.Queries
{
    public record GetMenuByMessAndDayQuery(
    int MessId,
    DayOfWeek Day
) : IRequest<MenuResponseDto>;
}
