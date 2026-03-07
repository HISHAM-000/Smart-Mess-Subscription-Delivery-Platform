using MediatR;
using MessMate.Application.Common.Exceptions;
using MessMate.Application.Features.RoleApplications.Commands;
using MessMate.Application.Interfaces.Repositories;
using MessMate.Application.Interfaces.Services;
using MessMate.Domain.Entities;
using MessMate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.RoleApplications.Handlers
{
    public class ApplyDeliveryPartnerHandler:IRequestHandler<ApplyDeliveryPartnerCommand,Unit>
    {
        private readonly IRoleApplicationRepository _applicationRepository;
        private readonly ICurrentUserService _currentUser;

        public ApplyDeliveryPartnerHandler(
            IRoleApplicationRepository applicationRepository,
            ICurrentUserService currentUser)
        {
            _applicationRepository = applicationRepository;
            _currentUser = currentUser;
        }

        public async Task<Unit>Handle(ApplyDeliveryPartnerCommand command,
            CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            var existingApplication = await _applicationRepository.GetUserPendingApplication(userId, UserRole.DeliveryPartner);

            if (existingApplication != null)
                throw new BadRequestException("You already have a pending MessOwner application.");
            var application = new RoleApplication
            {
                UserId = userId,
                RequestedRole = UserRole.DeliveryPartner
            };
            await _applicationRepository.AddAsync(application);
            return Unit.Value;
        }
    }
}
