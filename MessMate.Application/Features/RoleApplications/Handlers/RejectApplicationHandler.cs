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
    public class RejectApplicationHandler:IRequestHandler<RejectApplicationCommand,Unit>
    {
        private readonly IRoleApplicationRepository _repository;

        public RejectApplicationHandler(IRoleApplicationRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit>Handle(RejectApplicationCommand request,
            CancellationToken cancellationToken)
        {
            var application = await _repository.GetByIdAsync(request.ApplicationId);

            if (application == null)
                throw new NotFoundException("Application not found");

            application.Status =ApplicationStatus.Rejected;
            await _repository.UpdateAsync(application);
            return Unit.Value;
        }
    }
}
