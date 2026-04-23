using MediatR;
using MessMate.Application.Common.Exceptions;
using MessMate.Application.Features.Subscriptions.Commands;
using MessMate.Application.Interfaces.Services;
using MessMate.Domain.Enums;
using MessMate.Domain.Interfaces.Contracts;


namespace MessMate.Application.Features.Subscriptions.Handlers
{
    public class CancelSubscriptionCommandHandler
        : IRequestHandler<CancelSubscriptionCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public CancelSubscriptionCommandHandler(
            IUnitOfWork unitOfWork, ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(
        CancelSubscriptionCommand request, CancellationToken cancellationToken)
        {
            var subscription = await _unitOfWork.CustomerSubscriptions
            .GetByIdAsync(request.SubscriptionId);
            if (subscription == null)
                throw new NotFoundException("Subscription not found");

            if (subscription.CustomerId != _currentUser.UserId)
                throw new ForbiddenException("You cannot cancel this subscription.");

            if (subscription.Status == SubscriptionStatus.Cancelled)
                throw new BadRequestException("Subscription is already cancelled.");

            if (subscription.Status == SubscriptionStatus.Expired)
                throw new BadRequestException("Cannot cancel an expired subscription.");

            subscription.Status = SubscriptionStatus.Cancelled;
            subscription.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.CustomerSubscriptions.UpdateAsync(subscription);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
