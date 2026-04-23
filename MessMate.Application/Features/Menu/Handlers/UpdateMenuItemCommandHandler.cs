using MediatR;
using MessMate.Application.Common.Exceptions;
using MessMate.Application.Common.Policies;
using MessMate.Application.Features.Menu.Commands;
using MessMate.Application.Interfaces.Services;
using MessMate.Domain.Interfaces.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Menu.Handlers
{
    public class UpdateMenuItemCommandHandler
    : IRequestHandler<UpdateMenuItemCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public UpdateMenuItemCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(
        UpdateMenuItemCommand request,
        CancellationToken cancellationToken)
        {
            var item = await _unitOfWork.MenuItems.GetByIdAsync(request.ItemId);

            if (item == null || item.IsDeleted == true)
                throw new NotFoundException("Menu item not found");

            var mess = await _unitOfWork.Messes
                .GetByOwnerIdAsync(_currentUser.UserId)
                ?? throw new NotFoundException("Mess not found");

            var menu = await _unitOfWork.Menus.GetByIdAsync(item.MenuId);

            if (menu!.MessId != mess.Id)
                throw new ForbiddenException("Not your menu item");

            var isLocked = await MenuPolicy.IsMenuLockedAsync(
                _unitOfWork,
                mess.Id,
                menu.Day);

            if (isLocked)
                throw new BadRequestException("Menu locked");

            var existing = await _unitOfWork.MenuItems
                .GetByMenuIdAsync(menu.Id);

            if (existing.Any(x =>
                x.MealSlot == request.MealSlot &&
                x.Id != item.Id &&
                x.IsDeleted != true))
            {
                throw new AlreadyExistsException("Meal slot already exists");
            }

            item.Name = request.Name;
            item.Description = request.Description;
            item.MealSlot = request.MealSlot;
            item.IsVeg = request.IsVeg;
            item.IsAvailable = request.IsAvailable;
            item.UpdatedAt = DateTime.UtcNow;
            item.UpdatedBy = _currentUser.UserId;

            await _unitOfWork.MenuItems.UpdateAsync(item);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
