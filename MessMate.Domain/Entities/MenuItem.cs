using MessMate.Domain.Common;
using MessMate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Domain.Entities
{
    public class MenuItem : BaseEntity
    {
        public int Id { get; set; }
        public int MenuId { get; set; }

        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public MealSlot MealSlot { get; set; }

        public bool IsVeg { get; set; }
        public bool IsAvailable { get; set; }

        public Menu Menu { get; set; } = null!;
    }
}
