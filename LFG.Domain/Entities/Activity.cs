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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ActivityId { get; set; }
        [Required]
        [Display(Name = "Название")]
        public string ActivityName { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Описание")]
        public string ActivityDescription { get; set; }
        
        [Required]
        [Display(Name = "Дата создания")]
        [Column(TypeName = "datetime2")]
        public DateTime ActivityPostDayTime { get; set; }
        [Required]
        [Display(Name = "Дата Начала")]
        [Column(TypeName = "datetime2")]
        public DateTime ActivityStartDayTime { get; set; }
        [Column(TypeName = "datetime2")]
        [Display(Name = "Дата окончания")]
        public Nullable<DateTime> ActivityEndDayTime { get; set; }
        public Nullable<bool> IsCommited { get; set; }
        public virtual AppUser CommitCreator { get; set; }
        public virtual ActivityType ActivityTypeCurrent { get; set; }
        //public GeoLocation ActivityStartGEO { get; set; }
        //public GeoLocation ActivityEndGEO { get; set; }
        public virtual AppUser ActivityCreator { get; set; }


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
