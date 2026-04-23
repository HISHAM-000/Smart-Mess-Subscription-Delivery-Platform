using MediatR;
using MessMate.Application.Common.Exceptions;
using MessMate.Application.Features.Orders.Commands;
using MessMate.Application.Interfaces.Services;
using MessMate.Domain.Entities;
using MessMate.Domain.Enums;
using MessMate.Domain.Interfaces.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Orders.Handlers
{
    public class UpdateOrderStatusCommandHandler
       : IRequestHandler<UpdateOrderStatusCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public UpdateOrderStatusCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(
            UpdateOrderStatusCommand request,
            CancellationToken cancellationToken)
        {

            var order = await _unitOfWork.Orders
                .GetByIdWithDetailsAsync(request.OrderId)
                ?? throw new NotFoundException("Order not found");

            var staff = await _unitOfWork.Users
                .GetByIdAsync(_currentUser.UserId)
                ?? throw new NotFoundException("Staff not found");

            if (staff.Role != UserRole.MessOwner && staff.Role != UserRole.MessStaff)
            {
                throw new ForbiddenException("You are not allowed to update order status.");
            }

            if (staff.MessId != order.MessId)
                throw new ForbiddenException(
                    "You cannot update orders for this mess.");

            ValidateTransition(order.Status, request.NewStatus);

            order.Status = request.NewStatus;
            order.UpdatedAt = DateTime.UtcNow;
            order.UpdatedBy = _currentUser.UserId;

            await _unitOfWork.Orders.UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        private static void ValidateTransition(OrderStatus current, OrderStatus next)
        {
            var allowedTransitions = new Dictionary<OrderStatus, List<OrderStatus>>
    {
        { OrderStatus.Pending, new() { OrderStatus.Preparing } },
        { OrderStatus.Preparing, new() { OrderStatus.OutForDelivery } },
        { OrderStatus.OutForDelivery, new() { OrderStatus.Delivered } },
    };

            if (!allowedTransitions.TryGetValue(current, out var nextStates) ||
                !nextStates.Contains(next))
            {
                throw new BadRequestException(
                    $"Invalid transition from {current} to {next}.");
            }
        }
    }
}
