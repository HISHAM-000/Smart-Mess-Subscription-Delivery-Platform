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
    public class GetMenuByMessAndDayQueryHandler
    : IRequestHandler<GetMenuByMessAndDayQuery, MenuResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMenuByMessAndDayQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<MenuResponseDto> Handle(
        GetMenuByMessAndDayQuery request,
        CancellationToken cancellationToken)
        {
            var menu = await _unitOfWork.Menus
                .GetByMessAndDayAsync(request.MessId, request.Day);

            if (menu == null)
                throw new NotFoundException("Menu not found");

            var items = await _unitOfWork.MenuItems
                .GetByMenuIdAsync(menu.Id);

            return new MenuResponseDto
            {
                Id = menu.Id,
                Day = menu.Day,
                Items = items.Select(i => new MenuItemDto
                {
                    Id = i.Id,
                    Name = i.Name,
                    Description = i.Description,
                    MealSlot = i.MealSlot,
                    IsVeg = i.IsVeg,
                    IsAvailable = i.IsAvailable
                }).ToList()
            };
        }
    }
}
