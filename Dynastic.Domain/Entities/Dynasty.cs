using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynastic.Domain.Entities;

public class Dynasty : Base
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public List<Person>? Members { get; set; }
    public string? UserId { get; set; }
}
