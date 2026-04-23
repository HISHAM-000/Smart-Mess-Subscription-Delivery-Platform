using MediatR;
using MessMate.Application.Common.Exceptions;
using MessMate.Application.Common.Policies;
using MessMate.Application.Features.Menu.Commands;
using MessMate.Application.Interfaces.Services;
using MessMate.Domain.Entities;
using MessMate.Domain.Interfaces.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Menu.Handlers
{
    public class AddMenuItemCommandHandler
    : IRequestHandler<AddMenuItemCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public AddMenuItemCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<int> Handle(
        AddMenuItemCommand request,
        CancellationToken cancellationToken)
        {
            var menu = await _unitOfWork.Menus.GetByIdAsync(request.MenuId);

            if (menu == null || menu.IsDeleted == true)
                throw new NotFoundException("Menu not found");

            var mess = await _unitOfWork.Messes
                .GetByOwnerIdAsync(_currentUser.UserId);

            if (mess == null)
                throw new NotFoundException("Mess not found");

            if (menu.MessId != mess.Id)
                throw new ForbiddenException("You cannot modify this menu");

            var existingItems = await _unitOfWork.MenuItems
                .GetByMenuIdAsync(menu.Id);

            if (existingItems.Any(x =>
                x.MealSlot == request.MealSlot &&
                x.IsDeleted != true))
            {
                throw new AlreadyExistsException(
                    $"{request.MealSlot} already has a menu item");
            }

            var item = new MenuItem
            {
                MenuId = request.MenuId,
                Name = request.Name,             
                Description = request.Description,
                MealSlot = request.MealSlot,
                IsVeg = request.IsVeg,
                IsAvailable = request.IsAvailable,
                CreatedBy = _currentUser.UserId
            };

            await _unitOfWork.MenuItems.AddAsync(item);
            await _unitOfWork.SaveChangesAsync();

            return item.Id;
        }
    }
}
