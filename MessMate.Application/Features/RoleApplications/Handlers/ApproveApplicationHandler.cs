using MediatR;
using MessMate.Application.Common.Exceptions;
using MessMate.Application.Features.RoleApplications.Commands;
using MessMate.Application.Interfaces.Repositories;
using MessMate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.RoleApplications.Handlers
{
    public class ApproveApplicationHandler:IRequestHandler<ApproveApplicationCommand,Unit>
    {
        private readonly IRoleApplicationRepository _applicationRepository;
        private readonly IUserRepository _userRepository;
        public ApproveApplicationHandler(
        IRoleApplicationRepository applicationRepository,
        IUserRepository userRepository)
        {
            _applicationRepository = applicationRepository;
            _userRepository = userRepository;
        }

        public async Task<Unit>Handle(ApproveApplicationCommand request,
            CancellationToken cancellationToken)
        {
            var application = await _applicationRepository.GetByIdAsync(request.ApplicationId);
            if (application == null)
                throw new NotFoundException("Application not found");

            if (application.Status != ApplicationStatus.Pending)
                throw new Exception("Application already processed");

            application.Status = ApplicationStatus.Approved;

            var user = await _userRepository.GetByIdAsync(application.UserId);
            if (user == null)
                throw new NotFoundException("User not found");
            user.Role = application.RequestedRole;

            await _applicationRepository.UpdateAsync(application);
            await _userRepository.UpdateAsync(user);

            return Unit.Value;


        }
    }
}
