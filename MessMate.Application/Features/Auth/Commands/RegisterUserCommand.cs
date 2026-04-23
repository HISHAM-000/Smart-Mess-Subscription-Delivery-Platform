using MediatR;
using MessMate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Auth.Commands
{
    public record RegisterUserCommand(
        string name,
        string Email,
        string PhoneNumber,
        string Password) : IRequest<int>;

}
