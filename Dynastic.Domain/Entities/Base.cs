using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynastic.Domain.Entities;

public abstract class Base
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        return obj != null && obj.GetType() == GetType() && ((Base) obj).Id.Equals(this.Id);
    }
}
