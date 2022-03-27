using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynastic.Domain.Entities
{
    public class ChildRelationship
    {
        public Person Child { get; set; }
        public Guid ChildId { get; set; }
        public Relationship Relationship { get; set; }
        public Guid RelationshipId { get; set; }
    }
}
