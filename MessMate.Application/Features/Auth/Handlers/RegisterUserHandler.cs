using MediatR;
using MessMate.Application.Common.Exceptions;
using MessMate.Application.Features.Auth.Commands;
using MessMate.Domain.Entities;
using MessMate.Domain.Interfaces.Contracts;



namespace MessMate.Application.Features.Auth.Handlers
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        public RegisterUserHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(RegisterUserCommand request, CancellationToken token)
        {
            var email = request.Email.Trim();
            var name = request.name.Trim();
            var phone = request.PhoneNumber.Trim();
            var password = request.Password.Trim();

            var existingUser = await _unitOfWork.Users.GetByEmailAsync(email);
            if (existingUser != null)
                throw new AlreadyExistsException("User Already exists");

            var user = new User
            {
                Name = name,
                Email = email,
                PhoneNumber = phone,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Role = Domain.Enums.UserRole.Customer,
                IsActive = true,
            };
            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return user.Id;
        }
    }
}
