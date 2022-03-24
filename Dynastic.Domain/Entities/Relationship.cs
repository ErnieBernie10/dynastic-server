using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynastic.Domain.Entities
{
    public class Relationship : Base
    {
        public Person Person { get; set; }
        public Guid PersonId { get; set; }
        public Person Partner { get; set; }
        public Guid PartnerId { get; set; } 
    }
}
