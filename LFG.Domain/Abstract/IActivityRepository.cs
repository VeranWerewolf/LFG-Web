using System.Collections.Generic;
using LFG.Domain.Entities;

namespace LFG.Domain.Abstract
{
    public interface IActivityRepository
    {
        IEnumerable<Activity> Activities { get; }

        void SaveActivity (Activity activity);
        void Commit(Activity activity, AppUser user);

    }
}