using MediatR;
using MessMate.Application.Common.Exceptions;
using MessMate.Application.Features.Menu.DTOs;
using MessMate.Application.Features.Menu.Queries;
using MessMate.Domain.Interfaces.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Menu.Handlers
{
    public class GetTodayMenuQueryHandler
    : IRequestHandler<GetTodayMenuQuery, List<MenuItemDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTodayMenuQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<MenuItemDto>> Handle(
        GetTodayMenuQuery request,
        CancellationToken cancellationToken)
        {
            var today = DateTime.UtcNow.DayOfWeek;

            var menu = await _unitOfWork.Menus
                .GetByMessAndDayAsync(request.MessId, today);

            if (menu == null)
                throw new NotFoundException("No menu for today");

            var items = await _unitOfWork.MenuItems
                .GetByMenuIdAsync(menu.Id);

            return items.Select(i => new MenuItemDto
            {
                Id = i.Id,
                Name = i.Name,
                Description = i.Description,
                MealSlot = i.MealSlot,
                IsVeg = i.IsVeg,
                IsAvailable = i.IsAvailable
            }).ToList();
        }
    }
}
