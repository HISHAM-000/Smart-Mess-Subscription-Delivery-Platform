using MediatR;
using MessMate.Application.Features.Orders.Commands;
using MessMate.Application.Services;
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
    public class GenerateOrdersCommandHandler
    : IRequestHandler<GenerateOrdersCommand, int>
    {
        private readonly OrderGenerationService _orderService;
        private readonly IUnitOfWork _unitOfWork;

        public GenerateOrdersCommandHandler(OrderGenerationService orderService, IUnitOfWork unitOfWork)
        {
            _orderService = orderService;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(
            GenerateOrdersCommand request,
            CancellationToken ct)
        {
            var today = DateTime.Now.Date;
            var now = DateTime.Now;

            var count = await _orderService.GenerateOrdersForTodayAsync(
                today, now, ct);

            await _unitOfWork.SaveChangesAsync();

            return count;
        }
    }
}
