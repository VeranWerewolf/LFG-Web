using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LFG.Domain.Entities
{
    [Table("ActivityType")]
    public class ActivityType
    {
        [Key]
        public Guid ActivityTypeId { get; set; }
        public string ActivityTypeTitle { get; set; }

        public virtual ActivityType ActivityTypeParent { get; set; }
        public ICollection<ActivityType> ActivityTypeChildren { get; set; }

        public ICollection<Activity> ActivitiesInType { get; set; }

        public override string ToString()
        {
            return ActivityTypeTitle;
        }
    }
}
