using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Menu.Commands
{
    public record CreateMenuCommand(DayOfWeek Day) : IRequest<int>;
}
