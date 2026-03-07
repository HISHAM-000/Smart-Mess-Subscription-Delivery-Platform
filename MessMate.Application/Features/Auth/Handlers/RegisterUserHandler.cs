using MediatR;
using MessMate.Application.Common.Exceptions;
using MessMate.Application.Features.Auth.Commands;
using MessMate.Application.Interfaces.Repositories;
using MessMate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Auth.Handlers
{
    public class RegisterUserHandler:IRequestHandler<RegisterUserCommand,Guid>
    {
        private readonly IUserRepository _userRepository;
        public RegisterUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;      
        }

        public async Task<Guid> Handle(RegisterUserCommand request,CancellationToken token)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if(existingUser != null)
                throw new AlreadyExistsException("User Already exists");

            var user = new User
            {
                Name = request.name,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = request.Role
            };
            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();
            return user.Id;
        }
    }
}
