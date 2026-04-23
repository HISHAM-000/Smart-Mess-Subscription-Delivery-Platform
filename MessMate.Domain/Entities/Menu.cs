using MessMate.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Domain.Entities
{
    public class Menu : BaseEntity
    {
        public int Id { get; set; }
        public int MessId { get; set; }
        public DayOfWeek Day { get; set; }

        public Mess Mess { get; set; } = null!;
        public ICollection<MenuItem> Items { get; set; } = new List<MenuItem>();
    }
}
