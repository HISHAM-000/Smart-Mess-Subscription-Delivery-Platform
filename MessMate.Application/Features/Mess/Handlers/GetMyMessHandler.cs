using MediatR;
using MessMate.Application.Common.Exceptions;
using MessMate.Application.Features.Mess.DTOs;
using MessMate.Application.Features.Mess.Queries;
using MessMate.Application.Interfaces.Services;
using MessMate.Domain.Entities;
using MessMate.Domain.Interfaces.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Mess.Handlers
{
    public class GetMyMessHandler : IRequestHandler<GetMyMessQuery, GetMyMessResponseDto>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IUnitOfWork _unitOfWork;
        public GetMyMessHandler(ICurrentUserService currentUser, IUnitOfWork unitOfWork)
        {
            _currentUser = currentUser;
            _unitOfWork = unitOfWork;

        }
        public async Task<GetMyMessResponseDto> Handle(GetMyMessQuery request,
            CancellationToken cancellationToken)
        {
            var mess = await _unitOfWork.Messes.GetByOwnerIdAsync(_currentUser.UserId);
            if (mess == null || !mess.IsActive)
                throw new NotFoundException("Mess not found");

            if (!mess.IsActive)
                throw new ForbiddenException("Mess is inactive");
            return new GetMyMessResponseDto
            {
                Id = mess.Id,
                Name = mess.Name,
                Description = mess.Description,
                AddressLine = mess.AddressLine,
                City = mess.City,
                State = mess.State,
                PostalCode = mess.PostalCode,
                Latitude = mess.Latitude,
                Longitude = mess.Longitude,
                Rating = mess.Rating,
            };
        }
    }
}
