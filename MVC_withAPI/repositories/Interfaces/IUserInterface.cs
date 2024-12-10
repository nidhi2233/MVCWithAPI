using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using repositories.Models;

namespace repositories.Interfaces
{
    public interface IUserInterface
    {
        Task<int> Register(t_User user);
        Task<t_User> Login(vm_Login user);
    }
}