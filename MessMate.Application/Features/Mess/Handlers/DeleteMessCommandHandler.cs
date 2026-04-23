using MediatR;
using MessMate.Application.Common.Exceptions;
using MessMate.Application.Features.Mess.Commands;
using MessMate.Application.Interfaces.Services;
using MessMate.Domain.Interfaces.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Mess.Handlers
{
    public class DeleteMessCommandHandler : IRequestHandler<DeleteMessCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public DeleteMessCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(DeleteMessCommand request,
            CancellationToken cancellationToken)
        {
            var mess = await _unitOfWork.Messes.GetByIdAsync(request.id);
            if (mess == null)
                throw new NotFoundException("Mess not found");

            if (mess.OwnerId != _currentUser.UserId)
                throw new ForbiddenException("You are not allowed to delete this mess.");

            var hasActiveSubscriptions = await _unitOfWork.CustomerSubscriptions
                .HasActiveSubscriptionsByMessIdAsync(request.id);

            if (hasActiveSubscriptions)
                throw new ForbiddenException(
                    "Mess has active subscriptions. Cancel or wait for them to expire before deleting.");

            mess.IsDeleted = true;
            mess.DeletedOn = DateTime.UtcNow;
            mess.DeletedBy = _currentUser.UserId;
            mess.IsActive = false;
            mess.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Messes.UpdateAsync(mess);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
