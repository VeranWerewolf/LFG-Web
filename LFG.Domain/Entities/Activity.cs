using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LFG.Domain.Entities
{
    public enum ActivityAccessTypes
    {
        [Display(Name = "Открытое")]
        Public = 0,
        [Display(Name = "Закрытое")]
        Protected = 1,
        [Display(Name = "Приватное")]
        Private =2
    }

    [Table("Activities")]
    public class Activity
    {
        //Many-to-many Activities and Users
        public Activity()
        {
            this.AppUser = new HashSet<AppUser>();
        }
        public virtual ICollection<AppUser> AppUser { get; set; }

        [Key]
        public Guid ActivityId { get; set; }
        [Required]
        public string ActivityName { get; set; }
        [Required]
        public string ActivityDescription { get; set; }
        
        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime ActivityPostDayTime { get; set; }
        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime ActivityStartDayTime { get; set; }
        [Column(TypeName = "datetime2")]
        public Nullable<DateTime> ActivityEndDayTime { get; set; }
        public virtual ActivityType ActivityTypeCurrent { get; set; }
        //public GeoLocation ActivityStartGEO { get; set; }
        //public GeoLocation ActivityEndGEO { get; set; }
        public AppUser ActivityCreator { get; set; }


        [Required]
        public virtual int ActivityAccessType
        {
            get
            {
                return (int)this.ActivityAccess;
            }
            set
            {
                ActivityAccess = (ActivityAccessTypes)value;
            }
        }
        [EnumDataType(typeof(ActivityAccessTypes))]
        public ActivityAccessTypes ActivityAccess { get; set; }
        
    }
}
