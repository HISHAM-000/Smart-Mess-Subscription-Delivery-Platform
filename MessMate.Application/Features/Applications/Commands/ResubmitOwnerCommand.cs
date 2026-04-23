using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Applications.Commands
{
    public record ResubmitOwnerCommand(
        string Name,
        string Email,
        string PhoneNumber,
        string AuthorizedName,
        string LicenseNumber) : IRequest<int>;
}
