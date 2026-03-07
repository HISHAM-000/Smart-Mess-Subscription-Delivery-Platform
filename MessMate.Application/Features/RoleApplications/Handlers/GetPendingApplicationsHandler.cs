using MediatR;
using MessMate.Application.Features.RoleApplications.DTOs;
using MessMate.Application.Features.RoleApplications.Queries;
using MessMate.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.RoleApplications.Handlers
{
    public class GetPendingApplicationsHandler:IRequestHandler<GetPendingApplicationsQuery,
        List<RoleApplicationDto>>
    {
        private readonly IRoleApplicationRepository _repository;
        public GetPendingApplicationsHandler(IRoleApplicationRepository repository)
        {
            _repository = repository;
        }
        public async Task<List<RoleApplicationDto>>Handle(GetPendingApplicationsQuery request,
            CancellationToken cancellationToken)
        {
            var applications =await  _repository.GetPendingApplicationsAsync();
            return applications.Select(a => new RoleApplicationDto
            {
                Id = a.Id,
                UserId = a.UserId,
                RequestedRole = a.RequestedRole.ToString(),
                Status = a.Status.ToString()
            }).ToList();
        }
    }
}
