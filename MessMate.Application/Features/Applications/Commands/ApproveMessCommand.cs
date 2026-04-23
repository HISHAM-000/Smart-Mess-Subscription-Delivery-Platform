using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Applications.Commands
{
    public record ApproveMessCommand(int MessId) : IRequest<bool>;
}
