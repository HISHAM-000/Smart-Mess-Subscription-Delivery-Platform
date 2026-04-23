using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Mess.Commands
{
    //public class CreateMessCommand:IRequest<int>
    //{
    //    public int CategoryId { get; set; }

    //    public string Name { get; set; } = null!;

    //    public string Description { get; set; } = null!;

    //    public string AddressLine { get; set; } = null!;

    //    public string City { get; set; } = null!;

    //    public string State { get; set; } = null!;

    //    public string PostalCode { get; set; } = null!;

    //    public double Latitude { get; set; }

    //    public double Longitude { get; set; }

    //    public bool DeliveryAvailable { get; set; }
    //}
    public record CreateMessCommand(
        string Name,
        string? Description,
        string AddressLine,
        string City,
        string State,
        string PostalCode,
        double Latitude,
        double Longitude
        ):IRequest<int>;
}
