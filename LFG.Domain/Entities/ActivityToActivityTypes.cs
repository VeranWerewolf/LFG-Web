using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFG.Domain.Entities
{
    [Table("ActivitiesToActivityTypes")]
    public class ActivityToActivityTypes
    {
        public ActivityType ActivityTypeId { get; set; }
        public Activity ActivityID { get; set; }
    }
}
