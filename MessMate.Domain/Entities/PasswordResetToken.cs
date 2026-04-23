using MessMate.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessMate.Domain.Entities
{
    public class PasswordResetToken:BaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public string Token { get; set; } = default!;

        public DateTime ExpiresAt { get; set; }

        public bool IsUsed { get; set; } = false;

        public User User { get; set; } = null!;
    }
}
