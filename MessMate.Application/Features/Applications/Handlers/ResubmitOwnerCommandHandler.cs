using MediatR;
using MessMate.Application.Common.Exceptions;
using MessMate.Application.Features.Applications.Commands;
using MessMate.Domain.Enums;
using MessMate.Domain.Interfaces.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Applications.Handlers
{
    public class ResubmitOwnerCommandHandler: IRequestHandler<ResubmitOwnerCommand,int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ResubmitOwnerCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        public async Task<int> Handle(
        ResubmitOwnerCommand request,
        CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(request.Email);
            if (user == null)
                throw new NotFoundException("User not found");

            if (user.Role != UserRole.MessOwner)
                throw new ForbiddenException("Only mess owners can resubmit");

            if (!user.IsRejected)
                throw new BadRequestException("Your account is not rejected");

            user.Name = request.Name;
            user.PhoneNumber = request.PhoneNumber;
            user.AuthorisedName = request.AuthorizedName;
            user.LicenseNumber = request.LicenseNumber;

            user.IsRejected = false;
            user.RejectionReason = null;
            user.IsDeleted = false;
            user.IsActive = true;
            user.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return user.Id;
        }
    }
}
