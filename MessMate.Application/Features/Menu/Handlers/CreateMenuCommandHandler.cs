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
    public class CreateMenuCommandHandler
     : IRequestHandler<CreateMenuCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public CreateMenuCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<int> Handle(
        CreateMenuCommand request,
        CancellationToken cancellationToken)
        {
            var mess = await _unitOfWork.Messes
                .GetByOwnerIdAsync(_currentUser.UserId);

            if (mess == null)
                throw new NotFoundException("Mess not found");

            if (!mess.IsApproved)
                throw new ForbiddenException("Mess is not approved");

            var existingMenu = await _unitOfWork.Menus
                .GetByMessAndDayAsync(mess.Id, request.Day);

            if (existingMenu != null)
                throw new AlreadyExistsException($"Menu already exists for {request.Day}");

            var menu = new Domain.Entities.Menu
            {
                MessId = mess.Id,
                Day = request.Day,
                CreatedBy = _currentUser.UserId
            };

            await _unitOfWork.Menus.AddAsync(menu);
            await _unitOfWork.SaveChangesAsync();

            return menu.Id;
        }
    }
}
