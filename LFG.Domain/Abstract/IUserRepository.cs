using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LFG.Domain.Entities;

namespace LFG.Domain.Abstract
{
    public interface IUserRepository
    {
        IEnumerable<AppUser> AppUsers { get; }
    }
}
