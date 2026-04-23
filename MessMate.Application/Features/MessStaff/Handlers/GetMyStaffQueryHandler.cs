using MediatR;
using MessMate.Application.Common.Exceptions;
using MessMate.Application.Features.MessStaff.DTOs;
using MessMate.Application.Features.MessStaff.Queries;
using MessMate.Application.Interfaces.Services;
using MessMate.Domain.Interfaces.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.MessStaff.Handlers
{
    public class GetMyStaffQueryHandler
        : IRequestHandler<GetMyStaffQuery, List<StaffDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public GetMyStaffQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<List<StaffDto>> Handle(
            GetMyStaffQuery request,
            CancellationToken cancellationToken)
        {
            var mess = await _unitOfWork.Messes
                .GetByOwnerIdAsync(_currentUser.UserId)
                ?? throw new NotFoundException(
                    "No mess found for this owner.");

            var staff = await _unitOfWork.Users
                .GetStaffByMessIdAsync(mess.Id);

            return staff.Select(s => new StaffDto
            {
                Id = s.Id,
                Name = s.Name,
                Email = s.Email,
                PhoneNumber = s.PhoneNumber,
                IsActive = s.IsActive,
                JoinedOn = s.CreatedOn,
            }).ToList();
        }
    }
}
