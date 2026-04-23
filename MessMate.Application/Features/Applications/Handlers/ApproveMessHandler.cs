using MediatR;
using MessMate.Application.Common.Exceptions;
using MessMate.Application.Features.Applications.Commands;
using MessMate.Application.Features.Auth.Commands;
using MessMate.Application.Features.Mess.Commands;
using MessMate.Domain.Interfaces.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Applications.Handlers
{
    public class ApproveMessHandler : IRequestHandler<ApproveMessCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApproveMessHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        public async Task<bool> Handle(
        ApproveMessCommand request,
        CancellationToken cancellationToken)
        {
            var mess = await _unitOfWork.Messes.GetByIdAsync(request.MessId);
            if (mess == null)
                throw new NotFoundException("Mess not found");

            if (mess.IsApproved)
                throw new ConflictException("This mess is already approved.");

            mess.IsApproved = true;
            mess.IsActive = true;
            mess.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Messes.UpdateAsync(mess);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
