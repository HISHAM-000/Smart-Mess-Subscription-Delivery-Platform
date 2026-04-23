using MediatR;
using MessMate.Application.Common.Exceptions;
using MessMate.Application.Features.Mess.Commands;
using MessMate.Application.Interfaces.Services;
using MessMate.Domain.Entities;
using MessMate.Domain.Interfaces.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Mess.Handlers
{
    public class CreateMessCommandHandler : IRequestHandler<CreateMessCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public CreateMessCommandHandler(
            ICurrentUserService currenUser,
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currenUser;
        }

        public async Task<int> Handle(CreateMessCommand request, CancellationToken cancellationToken)
        {
            var owner = await _unitOfWork.Users.GetByIdAsync(_currentUser.UserId);
            if (owner == null)
                throw new NotFoundException("not found");

            if (!owner.IsActive)
                throw new NotFoundException("Your account is not yet approved by admin.");
            var exists = await _unitOfWork.Messes.ExistsByOwnerIdAsync(_currentUser.UserId);

            if (exists)
                throw new AlreadyExistsException("You have already created a mess.")
                    ;
            var mess = new Domain.Entities.Mess
            {
                OwnerId = owner.Id,
                AuthorisedName = owner.AuthorisedName!,
                LicenseNumber = owner.LicenseNumber!,
                Name = request.Name,
                Description = request.Description,
                AddressLine = request.AddressLine,
                City = request.City,
                State = request.State,
                PostalCode = request.PostalCode,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                Rating = 0,
                IsApproved = false,
                IsActive = false,
                CreatedBy = owner.Id,
            };

            await _unitOfWork.Messes.AddAsync(mess);
            await _unitOfWork.SaveChangesAsync();

            return mess.Id;
        }
    }
}
