using MediatR;
using MessMate.Application.Common.Exceptions;
using MessMate.Application.Features.Delivery.Commands;
using MessMate.Application.Interfaces.Services;
using MessMate.Domain.Entities;
using MessMate.Domain.Enums;
using MessMate.Domain.Interfaces.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Delivery.Handlers
{
    public class ConfirmDeliveryCommandHandler
        : IRequestHandler<ConfirmDeliveryCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public ConfirmDeliveryCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(
            ConfirmDeliveryCommand request,
            CancellationToken cancellationToken)
        {
            var delivery = await _unitOfWork.Deliveries
                .GetByOrderIdAsync(request.OrderId)
                ?? throw new NotFoundException(
                    "No delivery found for this order.");

            if (delivery.StaffId != _currentUser.UserId)
                throw new ForbiddenException(
                    "You are not assigned to this delivery.");

            if (delivery.Status == DeliveryStatus.Delivered)
                throw new BadRequestException(
                    "This delivery is already confirmed.");

            if (delivery.OTP.Trim() != request.OTP.Trim())
                throw new BadRequestException(
                    "Invalid OTP. Please check with the customer.");

            delivery.Status = DeliveryStatus.Delivered;
            delivery.DeliveredAt = DateTime.UtcNow;
            delivery.UpdatedAt = DateTime.UtcNow;
            delivery.UpdatedBy = _currentUser.UserId;


            var order = await _unitOfWork.Orders
                .GetByIdAsync(request.OrderId)
                ?? throw new NotFoundException(
                    "Order not found");

            order.Status = OrderStatus.Delivered;
            order.UpdatedAt = DateTime.UtcNow;
            order.UpdatedBy = _currentUser.UserId;

            await _unitOfWork.Deliveries.UpdateAsync(delivery);
            await _unitOfWork.Orders.UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
