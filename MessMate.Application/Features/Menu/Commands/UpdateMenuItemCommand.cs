using MediatR;
using MessMate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Menu.Commands
{
    public record UpdateMenuItemCommand(
    int ItemId,
    string Name,
    string? Description,
    MealSlot MealSlot,
    bool IsVeg,
    bool IsAvailable
) : IRequest<bool>;
}
