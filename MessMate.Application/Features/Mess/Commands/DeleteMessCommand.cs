using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Mess.Commands
{
    public class DeleteMessCommand:IRequest<bool>
    {
        public int id { get; set; }
    }
}
