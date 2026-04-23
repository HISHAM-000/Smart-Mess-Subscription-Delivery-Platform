using MediatR;
using MessMate.Application.Common.Exceptions;
using MessMate.Application.Features.Mess.DTOs;
using MessMate.Application.Features.Mess.Queries;
using MessMate.Application.Interfaces.Services;
using MessMate.Domain.Enums;
using MessMate.Domain.Interfaces.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Mess.Handlers
{
    public class GetAllMessQueryHandler : IRequestHandler<GetAllMessesQuery, List<GetAllResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;
        public GetAllMessQueryHandler(IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }
        public async Task<List<GetAllResponseDto>> Handle(GetAllMessesQuery request,
            CancellationToken cancellationToken)
        {
            if (_currentUser.IsAuthenticated &&
                _currentUser.Role == nameof(UserRole.MessOwner))
                throw new ForbiddenException("Access denied for mess owners.");

            var isAdmin = _currentUser. Role == nameof(UserRole.Admin);
            var messes = await _unitOfWork.Messes.GetAllMessesAsync(
                approvedOnly: !isAdmin);
            return messes.Select(m => new GetAllResponseDto
            {
                Id = m.Id,
                Name = m.Name,
                City = m.City,
                State = m.State,
                Rating = m.Rating,
            }).ToList();
        }
    }
}


