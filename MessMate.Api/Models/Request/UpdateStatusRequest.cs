using MessMate.Domain.Enums;

namespace MessMate.Api.Models.Request
{
    public class UpdateStatusRequest
    {
        public OrderStatus NewStatus { get; set; }
    }
}
