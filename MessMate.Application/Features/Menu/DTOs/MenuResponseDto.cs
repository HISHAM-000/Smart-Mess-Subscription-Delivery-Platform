using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Application.Features.Menu.DTOs
{
    public class MenuResponseDto
    {
        public int Id { get; set; }
        public DayOfWeek Day { get; set; }
        public List<MenuItemDto> Items { get; set; } = new();
    }
}
