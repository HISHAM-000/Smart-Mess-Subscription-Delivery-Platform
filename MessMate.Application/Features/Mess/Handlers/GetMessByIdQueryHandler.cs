using MediatR;
using MessMate.Application.Common.Exceptions;
using MessMate.Application.Features.Mess.DTOs;
using MessMate.Application.Features.Messes.Queries;
using MessMate.Application.Interfaces.Services;
using MessMate.Domain.Entities;
using MessMate.Domain.Enums;
using MessMate.Domain.Interfaces.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Mess.Handlers
{
    public class GetMessByIdQueryHandler : IRequestHandler<GetMessByIdQuery, GetByIdResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;
        public GetMessByIdQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<GetByIdResponseDto> Handle(GetMessByIdQuery request,
            CancellationToken cancellationToken)
        {
            var mess = await _unitOfWork.Messes.GetByIdAsync(request.Id);
            if (mess == null)
                throw new NotFoundException("Mess not found");

            bool isAdmin = _currentUser.Role == UserRole.Admin.ToString();

            if (!isAdmin && !mess.IsApproved)
                throw new NotFoundException("Mess not found");

            return new GetByIdResponseDto
            {
                Id = mess.Id,
                Name = mess.Name,
                Description = mess.Description,
                City = mess.City,
                State = mess.State,
                PostalCode = mess.PostalCode,
                Latitude = mess.Latitude,
                Longitude = mess.Longitude,
                Rating = mess.Rating
            };
        }
    }
}
