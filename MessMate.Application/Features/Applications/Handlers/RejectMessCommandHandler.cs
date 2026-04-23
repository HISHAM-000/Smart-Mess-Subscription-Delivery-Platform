using MediatR;
using MessMate.Application.Common.Exceptions;
using MessMate.Application.Features.Applications.Commands;
using MessMate.Domain.Interfaces.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Applications.Handlers
{
    public class RejectMessCommandHandler : IRequestHandler<RejectMessCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RejectMessCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            RejectMessCommand request,
            CancellationToken cancellationToken)
        {
            var mess = await _unitOfWork.Messes.GetByIdAsync(request.MessId)
                ?? throw new NotFoundException("Mess not found");

            if (mess.IsApproved)
                throw new ConflictException("Cannot reject an already approved mess.");

            if (mess.IsRejected)
                throw new ConflictException("This mess has already been rejected.");

            mess.IsRejected = true;
            mess.RejectionReason = request.Reason;
            mess.IsDeleted = true;     
            mess.DeletedOn = DateTime.UtcNow;
            mess.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Messes.UpdateAsync(mess);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
