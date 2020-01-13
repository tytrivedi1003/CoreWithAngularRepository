using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Data;
namespace Interfaces
{
    public interface ILoginService
    {
        Task<string> CreateToken(UserDetails userDetails);
    }
}
