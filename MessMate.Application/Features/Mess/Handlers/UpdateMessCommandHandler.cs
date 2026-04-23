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
    public class UpdateMessCommandHandler : IRequestHandler<UpdateMessCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;
        public UpdateMessCommandHandler(IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;

        }
        public async Task<Unit> Handle(UpdateMessCommand request,
            CancellationToken cancellationToken)
        {
            var mess = await _unitOfWork.Messes.GetByIdAsync(request.Id);
            if (mess == null)
                throw new NotFoundException("Mess not found");

            if (mess.OwnerId != _currentUser.UserId)
                throw new UnauthorizedException("You are not authorized to update this mess.");

            mess.Name = request.Name;
            mess.Description = request.Description;
            mess.AddressLine = request.AddressLine;
            mess.City = request.City;
            mess.State = request.State;
            mess.PostalCode = request.PostalCode;
            mess.Latitude = request.Latitude;
            mess.Longitude = request.Longitude;

            await _unitOfWork.SaveChangesAsync();
            return Unit.Value;
        }
    }
}
