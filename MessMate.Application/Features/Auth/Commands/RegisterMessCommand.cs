using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Auth.Commands
{
    public record RegisterMessCommand(
        string Name,
        string Email,
        string PhoneNumber,
        string Password,
        //string MessName,
        string AuthorizedName,
        string LicenseNumber):IRequest<int>;
}
