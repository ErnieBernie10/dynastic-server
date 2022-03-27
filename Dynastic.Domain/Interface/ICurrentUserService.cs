using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynastic.Domain.Interface
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
    }
}
