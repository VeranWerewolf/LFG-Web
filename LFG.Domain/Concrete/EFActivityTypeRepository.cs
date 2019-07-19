﻿using System.Collections.Generic;
using LFG.Domain.Entities;
using LFG.Domain.Abstract;

namespace LFG.Domain.Concrete
{
    public class EFActivityTypeRepository : IActivityTypeRepository
    {
        EFDbContext context = new EFDbContext();

        public IEnumerable<ActivityType> ActivityTypes
        {
            get { return context.ActivityTypes; }
        }
    }
}
