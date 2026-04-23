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
    public class AssignDeliveryCommandHandler
        : IRequestHandler<AssignDeliveryCommand, AssignDeliveryResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;
        public AssignDeliveryCommandHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<AssignDeliveryResult> Handle(
           AssignDeliveryCommand request,
           CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.Orders
                .GetByIdWithDetailsAsync(request.OrderId);

            if(order == null)
                throw new NotFoundException("Order not found");

            var mess = await _unitOfWork.Messes
                .GetByOwnerIdAsync(_currentUser.UserId);

            if(mess == null)
                throw new NotFoundException("Mess not found for this owner.");

            if (order.MessId != mess.Id)
                throw new ForbiddenException("This order does not belong to your mess.");

            if (order.Status != OrderStatus.Preparing)
                throw new BadRequestException("Order must be in Preparing status before assigning delivery.");

            var alreadyAssigned = await _unitOfWork.Deliveries
                .ExistsForOrderAsync(request.OrderId);

            if (alreadyAssigned)
                throw new BadRequestException("A delivery is already assigned for this order.");

            var staff = await _unitOfWork.Users
                .GetByIdAsync(request.StaffId);

            if(staff == null)
                throw new NotFoundException("Staff not found");

            if (staff.Role != UserRole.MessStaff)
                throw new ForbiddenException("User is not a mess staff member.");

            if (staff.MessId != mess.Id)
                throw new UnauthorizedException("This staff member does not belong to your mess.");

            var otp = GenerateOTP();

            var delivery = new Domain.Entities.Delivery
            {
                OrderId = order.Id,
                StaffId = request.StaffId,
                AssignedBy = _currentUser.UserId,
                Status = DeliveryStatus.Assigned,
                OTP = otp,
                AssignedAt = DateTime.UtcNow,
                DeliveryAddress = order.Subscription.DeliveryAddress,
                CreatedBy = _currentUser.UserId,
            };

            order.Status = OrderStatus.OutForDelivery;
            order.UpdatedAt = DateTime.UtcNow;
            order.UpdatedBy = _currentUser.UserId;

            await _unitOfWork.Deliveries.AddAsync(delivery);
            await _unitOfWork.Orders.UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return new AssignDeliveryResult(
                delivery.Id,
                otp,
                "Delivery assigned successfully.");
        }

        private static string GenerateOTP()
        {
            var random = new Random(Guid.NewGuid().GetHashCode());
            return random.Next(100000, 999999).ToString();
        }
    }
}

