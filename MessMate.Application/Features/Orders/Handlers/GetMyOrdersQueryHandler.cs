using MediatR;
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
    public class GetMyOrdersQueryHandler
       : IRequestHandler<GetMyOrdersQuery, List<OrderDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public GetMyOrdersQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<List<OrderDto>> Handle(
            GetMyOrdersQuery request,
            CancellationToken cancellationToken)
        {
            var orders = await _unitOfWork.Orders
                .GetByCustomerIdAsync(_currentUser.UserId);

            return orders.Select(o => new OrderDto
            {
                Id = o.Id,
                MessName = o.Mess.Name,
                OrderDate = o.OrderDate,
                MealSlot = o.MealSlot.ToString(),
                Amount = o.Amount,
                Status = o.Status.ToString(),
                DishName = o.MenuItem != null
                ? o.MenuItem.Name
                : "Not Assigned"
            }).ToList();
        }
    }
}
