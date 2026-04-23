using MediatR;
using MessMate.Application.Features.Applications.DTOs;
using MessMate.Application.Features.Applications.Queries;
using MessMate.Domain.Interfaces.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Applications.Handlers
{
    public class GetPendingMessesQueryHandler
       : IRequestHandler<GetPendingMessesQuery, List<PendingMessDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPendingMessesQueryHandler(IUnitOfWork unitOfWork)
            => _unitOfWork = unitOfWork;

        public async Task<List<PendingMessDto>> Handle(
            GetPendingMessesQuery request,
            CancellationToken cancellationToken)
        {
            var messes = await _unitOfWork.Messes
                .GetPendingMessesAsync();

            return messes.Select(m => new PendingMessDto
            {
                Id = m.Id,
                Name = m.Name,
                MessName = m.Name,
                AuthorisedName = m.AuthorisedName,
                LicenseNumber = m.LicenseNumber,
                AddressLine = m.AddressLine,
                City = m.City,
                State = m.State,
                PostalCode = m.PostalCode,
                Latitude = m.Latitude,
                Longitude = m.Longitude,
                OwnerName = m.Owner.Name,
                OwnerEmail = m.Owner.Email,
                CreatedOn = m.CreatedOn,
            }).ToList();
        }
    }
}
