using MediatR;
using MessMate.Application.Common.Exceptions;
using MessMate.Application.Features.MessStaff.Commands;
using MessMate.Application.Interfaces.Services;
using MessMate.Domain.Interfaces.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.MessStaff.Handlers
{
    public class DeleteStaffCommandHandler 
        : IRequestHandler<DeleteStaffCommand,bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public DeleteStaffCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUser)
        {
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;
        }
        
        public async Task<bool> Handle(DeleteStaffCommand request,
            CancellationToken cancellationToken)
        {
            var mess = await _unitOfWork.Messes.GetByOwnerIdAsync(_currentUser.UserId);

            if (mess == null)
                throw new NotFoundException("Mess not found for this user");

            var staff = await _unitOfWork.Users.GetByIdAsync(request.StaffId);

            if (staff == null)
                throw new NotFoundException("Staff not found");

            if (staff.IsDeleted == true)
                throw new ConflictException("Staff is already deleted");

            if (staff.Role != Domain.Enums.UserRole.MessStaff)
                throw new BadRequestException("This user is not a staff member.");

            if (staff.MessId != mess.Id)
                throw new ForbiddenException("This staff member does not belong to your mess.");

            staff.IsDeleted = true;
            staff.IsActive = false;
            staff.DeletedOn = DateTime.UtcNow;
            staff.DeletedBy = _currentUser.UserId;
            staff.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Users.UpdateAsync(staff);
            await _unitOfWork.SaveChangesAsync();

            return true;

        }
    }
}
