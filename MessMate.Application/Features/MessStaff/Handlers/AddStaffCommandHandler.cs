using MediatR;
using MessMate.Application.Common.Exceptions;
using MessMate.Application.Features.MessStaff.Commands;
using MessMate.Application.Interfaces.Services;
using MessMate.Domain.Entities;
using MessMate.Domain.Enums;
using MessMate.Domain.Interfaces.Contracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.MessStaff.Handlers
{
    public class AddStaffCommandHandler : IRequestHandler<AddStaffCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public AddStaffCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<int> Handle(
            AddStaffCommand request,
            CancellationToken cancellationToken)
        {
            var email = request.Email.Trim();
            var name = request.Name.Trim();
            var phone = request.PhoneNumber.Trim();
            var password = request.Password.Trim();

            var mess = await _unitOfWork.Messes
                .GetByOwnerIdAsync(_currentUser.UserId)
                ?? throw new NotFoundException("No mess found for this owner.");

            if (!mess.IsApproved)
                throw new UnauthorizedException(
                    "Your mess is not approved yet. Cannot add staff.");

            var existingUser = await _unitOfWork.Users
                 .GetByEmailAsync(email);

            if (existingUser != null)
                throw new AlreadyExistsException("A user with this email already exists.");

            var staff = new User
            {
                Name = name,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                PhoneNumber = phone,
                Role = UserRole.MessStaff,
                MessId = mess.Id,
                IsActive = true,
                CreatedBy = _currentUser.UserId,
            };


            await _unitOfWork.Users.AddAsync(staff);
            await _unitOfWork.SaveChangesAsync();

            return staff.Id;
        }
    }
}