using MediatR;
using MessMate.Application.Common.Exceptions;
using MessMate.Application.Features.Applications.Commands;
using MessMate.Application.Interfaces.Repositories;
using MessMate.Application.Interfaces.Services;
using MessMate.Domain.Entities;
using MessMate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Applications.Handlers
{
    public class ApplyMessOwnerHandler:IRequestHandler<ApplyMessOwnerCommand,Unit>
    {
        private readonly IRoleApplicationRepository _applicationRepository;
        private readonly ICurrentUserService _currentUserService;
        public ApplyMessOwnerHandler(IRoleApplicationRepository applicationRepository,
            ICurrentUserService currentUserService)
        {
            _applicationRepository = applicationRepository;
            _currentUserService = currentUserService;
        }
        
        public async Task<Unit>Handle(ApplyMessOwnerCommand command,CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            var existingApplication =await _applicationRepository.GetUserPendingApplication(userId, UserRole.MessOwner);

            if (existingApplication != null)
                throw new BadRequestException("You already have a pending MessOwner application.");

            var application = new RoleApplication
            {
                UserId = userId,
                RequestedRole = UserRole.MessOwner
            };
            await _applicationRepository.AddAsync(application);
            return Unit.Value;
        }
    }
}
