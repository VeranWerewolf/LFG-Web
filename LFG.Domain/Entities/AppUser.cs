﻿using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFG.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        //add your custom properties which have not included in IdentityUser before

        //Many-to-many Activities and Users
        public AppUser()
        {
            this.Activity = new HashSet<Activity>();
        }
        public virtual ICollection<Activity> Activity { get; set; }
    }

}
