using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynastic.Domain.Entities
{
    public class Person : Base
    {
        public string Firstname { get; set; }
        public string? Middlename { get; set; }
        public string Lastname { get; set; }
        public Person? Mother { get; set; }
        public Guid? MotherId { get; set; }
        public Person? Father { get; set; }
        public Guid? FatherId { get; set; }
        public DateTime? BirthDate { get; set; }
        public ICollection<ChildRelationship> Relationships { get; set; }
    }
}
