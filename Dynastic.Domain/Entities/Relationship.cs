using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynastic.Domain.Entities;

public class Relationship
{
    public Guid PersonId { get; set; }
    public Guid? PartnerId { get; set; }
    public List<string>? Children { get; set; }
}
