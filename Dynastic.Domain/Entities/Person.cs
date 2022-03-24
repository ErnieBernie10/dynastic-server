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
        public List<Relationship> Relationships { get; set; }

        protected virtual List<Person> MothersChildren { get; set; }
        protected virtual List<Person> FathersChildren { get; set; }
        
        [NotMapped]
        public List<Guid> Children => CombineChildren();

        private List<Guid> CombineChildren()
        {
            var list = new List<Person>();
            list.AddRange(MothersChildren ?? new List<Person>());
            list.AddRange(FathersChildren ?? new List<Person>());
            return list.Select(l => l.Id).ToList();
        }
    }
}
