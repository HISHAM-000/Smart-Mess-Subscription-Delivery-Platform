using MediatR;
using MessMate.Application.Common.Exceptions;
using MessMate.Application.Features.Auth.Commands;
using MessMate.Domain.Entities;
using MessMate.Domain.Interfaces.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Auth.Handlers
{
    public class RegisterMessHandler : IRequestHandler<RegisterMessCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        public RegisterMessHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<int> Handle(RegisterMessCommand request,
            CancellationToken cancellationToken)
        {
            var email = request.Email.Trim();
            var name = request.Name.Trim();
            var phone = request.PhoneNumber.Trim();
            var password = request.Password.Trim();
            //var mess = request.MessName;
            var authorisedName = request.AuthorizedName.Trim();
            var licenseNumber = request.LicenseNumber.Trim();

            var existingEmail = await _unitOfWork.Users.GetByEmailAsync(request.Email);
            if (existingEmail != null)
                throw new AlreadyExistsException("Email already exists");

            var license = await _unitOfWork.Users.GetByLicenseNumberAsync(request.LicenseNumber);
            if (license != null)
                throw new AlreadyExistsException("License number already exists");

            var user = new User
            {
                Name = name,
                Email = email,
                PhoneNumber = phone,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                MessId = null,
                AuthorisedName = authorisedName,
                LicenseNumber = request.LicenseNumber,
                Role = Domain.Enums.UserRole.MessOwner,
                IsActive = true
            };
            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return user.Id;
        }
    }
}
