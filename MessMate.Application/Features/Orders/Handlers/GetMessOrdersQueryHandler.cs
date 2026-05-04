using MediatR;
using MessMate.Application.Common.Exceptions;
using MessMate.Application.Features.Orders.DTOs;
using MessMate.Application.Features.Orders.Queries;
using MessMate.Application.Interfaces.Services;
using MessMate.Domain.Interfaces.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Orders.Handlers
{
    public class GetMessOrdersQueryHandler
       : IRequestHandler<GetMessOrdersQuery, List<MessOrderDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public GetMessOrdersQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<List<MessOrderDto>> Handle(
            GetMessOrdersQuery request,
            CancellationToken cancellationToken)
        {

            var mess = await _unitOfWork.Messes
                .GetByOwnerIdAsync(_currentUser.UserId);

            if (mess == null)
            {
                mess = await _unitOfWork.Messes
                    .GetByStaffIdAsync(_currentUser.UserId);
            }

            if (mess == null)
                throw new NotFoundException("Mess not found for this user.");


            var date = string.IsNullOrEmpty(request.Date)
                ? DateTime.UtcNow.Date
                : DateTime.Parse(request.Date).Date;

            var orders = await _unitOfWork.Orders
                .GetByMessIdAndDateAsync(mess.Id, date);

            return orders.Select(o => new MessOrderDto
            {
                Id = o.Id,
                CustomerName = o.Customer.Name,
                DeliveryAddress = o.Subscription.DeliveryAddress,
                MealSlot = o.MealSlot.ToString(),
                Status = (int)o.Status,
                Amount = o.Amount,
                OrderDate = o.OrderDate,
            }).ToList();
        }
    }
}
