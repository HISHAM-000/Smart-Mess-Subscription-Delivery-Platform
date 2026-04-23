using MediatR;
using MessMate.Application.Common.Exceptions;
using MessMate.Application.Features.Applications.Commands;
using MessMate.Domain.Entities;
using MessMate.Domain.Enums;
using MessMate.Domain.Interfaces.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Applications.Handlers
{
    public class RejectOwnerCommandHandler : IRequestHandler<RejectOwnerCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RejectOwnerCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            RejectOwnerCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.UserId)
                ?? throw new NotFoundException("User not found");

            //if (user.Role != UserRole.MessOwner)
            //    throw new ConflictException("Only mess owner accounts can be rejected.");

            //if (user.IsActive)
            //    throw new ConflictException("Cannot reject an already approved account.");

            if (user.IsRejected)
                throw new ConflictException("This Account is already rejected");

            user.IsRejected = true;
            user.RejectionReason = request.Reason;
            user.IsActive = false;
            user.IsDeleted = true;      
            user.DeletedOn = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
