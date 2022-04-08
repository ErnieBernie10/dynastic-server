using Dynastic.Domain.Common.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynastic.Domain.Entities;

public class UserDynasty
{
    public string? UserId { get; set; }
    public Guid DynastyId { get; set; }
    public Dynasty? Dynasty { get; set; }
}
