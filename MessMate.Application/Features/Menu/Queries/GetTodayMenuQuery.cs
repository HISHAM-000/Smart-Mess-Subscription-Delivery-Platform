using MediatR;
using MessMate.Application.Features.Menu.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Menu.Queries
{
    public record GetTodayMenuQuery(int MessId)
    : IRequest<List<MenuItemDto>>;
}
