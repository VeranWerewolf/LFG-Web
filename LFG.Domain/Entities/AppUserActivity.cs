using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFG.Domain.Entities
{
    public class AppUserActivity
    {
        [Key, Column(Order = 0)]
        public Guid ApplicationUserId { get; set; }
        [Key, Column(Order = 1)]
        public Guid ActivityId { get; set; }

        public virtual AppUser AppUser { get; set; }
        public virtual Activity Activity { get; set; }
    }
}
