using MediatR;
using MessMate.Application.Features.Delivery.DTOs;
using MessMate.Application.Features.Delivery.Queries;
using MessMate.Application.Interfaces.Services;
using MessMate.Domain.Interfaces.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Delivery.Handlers
{
    public class GetMyDeliveryHistoryQueryHandler
       : IRequestHandler<GetMyDeliveryHistoryQuery, List<DeliveryHistoryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public GetMyDeliveryHistoryQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<List<DeliveryHistoryDto>> Handle(
            GetMyDeliveryHistoryQuery request,
            CancellationToken cancellationToken)
        {
            var deliveries = await _unitOfWork.Deliveries
                .GetByStaffIdAsync(_currentUser.UserId);

            return deliveries.Select(d => new DeliveryHistoryDto
            {
                Id = d.Id,
                OrderId = d.OrderId,
                MessName = d.Order.Mess.Name,
                MealSlot = d.Order.MealSlot.ToString(),
                DeliveryAddress = d.DeliveryAddress,
                Status = d.Status.ToString(),
                AssignedAt = d.AssignedAt,
                DeliveredAt = d.DeliveredAt,
            }).ToList();
        }
    }
}
