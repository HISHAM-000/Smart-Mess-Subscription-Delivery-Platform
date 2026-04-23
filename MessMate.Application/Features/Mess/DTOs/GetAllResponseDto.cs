using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Mess.DTOs
{
    public class GetAllResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string City { get; set; } = null!;
        public string State { get; set; } = null!;
        public double Rating { get; set; }

    }
}
