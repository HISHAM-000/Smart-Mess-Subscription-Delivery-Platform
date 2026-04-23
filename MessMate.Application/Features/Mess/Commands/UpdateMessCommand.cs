using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Mess.Commands
{
    public record UpdateMessCommand(
        int Id,
        string Name,
        string Description,
        string AddressLine,
        string City,
        string State,
        string PostalCode,
        double Latitude,
        double Longitude
        ) : IRequest<Unit>;
}
