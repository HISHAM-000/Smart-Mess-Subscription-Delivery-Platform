using MediatR;
using MessMate.Application.Features.Subscriptions.DTOs;
using MessMate.Application.Features.Subscriptions.Queries;
using MessMate.Application.Interfaces.Services;
using MessMate.Domain.Interfaces.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Subscriptions.Commands
{
    public class GetMySkipsQueryHandler
       : IRequestHandler<GetMySkipsQuery, List<MealSkipDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public GetMySkipsQueryHandler(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<List<MealSkipDto>> Handle(
            GetMySkipsQuery request,
            CancellationToken cancellationToken)
        {
            var skips = await _unitOfWork.MealSkips
                .GetByCustomerIdAsync(_currentUser.UserId);

            return skips.Select(s => new MealSkipDto
            {
                Id = s.Id,
                OrderId = s.OrderId,
                MealSlot = s.MealSlot,
                MealDate = s.MealDate,
                RefundAmount = s.RefundAmount,
                RefundStatus = s.RefundStatus.ToString(),
                SkippedOn = s.CreatedOn,
            }).ToList();
        }
    }
}
