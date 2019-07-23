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
        //Many-to-many Activities and Users переделать
        public Activity()
        {
            this.ActivityPostDayTime = DateTime.Now;
            this.IsCommited = null;
        }
        public virtual ICollection<AppUserActivity> AppUserActivities { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ActivityId { get; set; }
        [Required(ErrorMessage = "Пожалуйста, введите название мероприятия")]
        [Display(Name = "Название")]
        public string ActivityName { get; set; }
        [Required(ErrorMessage = "Пожалуйста, введите короткое описание")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Описание")]
        public string ActivityDescription { get; set; }
        [Display(Name = "Дата создания")]
        [Column(TypeName = "datetime2")]
        public DateTime ActivityPostDayTime { get; set; }
        [Required(ErrorMessage = "Пожалуйста, выберите дату и время начала")]
        [Display(Name = "Дата Начала")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yy H:mm:ss tt}"), DataType(DataType.DateTime)]
        public DateTime ActivityStartDayTime { get; set; }
        [Display(Name = "Дата окончания")]
        public Nullable<DateTime> ActivityEndDayTime { get; set; }
        [Display(Name = "Одобрено")]
        public Nullable<bool> IsCommited { get; set; }
        [Display(Name = "Одобрил")]
        public virtual AppUser CommitCreator { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Комментарий к одобрению")]
        public string CommitDescription { get; set; }
        [Display(Name = "Категория")]
        public virtual ActivityType ActivityTypeCurrent { get; set; }
        //public GeoLocation ActivityStartGEO { get; set; }
        //public GeoLocation ActivityEndGEO { get; set; }
        [Display(Name = "Создатель")]
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

        [Display(Name = "Уровень доступа")]
        [EnumDataType(typeof(ActivityAccessTypes))]
        public ActivityAccessTypes ActivityAccess { get; set; }
        
    }
}
