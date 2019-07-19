using System.Collections.Generic;
using LFG.Domain.Entities;
using LFG.Domain.Abstract;

namespace LFG.Domain.Concrete
{
    public class EFActivityRepository : IActivityRepository
    {
        EFDbContext context = new EFDbContext();

        public IEnumerable<Activity> Activities
        {
            get { return context.Activities; }
        }
    }
}