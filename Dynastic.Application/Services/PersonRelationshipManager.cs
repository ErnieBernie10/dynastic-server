using Dynastic.Application.Common.Interfaces;
using Dynastic.Domain.Entities;

namespace Dynastic.Application.Services;

public class PersonRelationshipManager : IPersonRelationshipManager
{
    private Dynasty _dynasty;
    public PersonRelationshipManager(Dynasty dynasty)
    {
        _dynasty = dynasty;
    }
    public Relationship PairPartner(Person person, Person partner)
    {
        var relationship = FindRelationship(_dynasty, person, partner);
        if (relationship is not null)
        {
            // This relationship already exists
            return relationship;
        }

        var newRelationship = new Relationship() { PersonId = person.Id, PartnerId = partner.Id };

        _dynasty.Relationships.Add(newRelationship);

        return newRelationship;
    }

    public Relationship AddChild(Person child, Person person, Person partner)
    {
        var relationship = PairPartner(person, partner);
        relationship.Children.Add(child.Id.ToString());
        return relationship;
    }

    public Relationship AddChild(Person child, Person person)
    {
        var singleRelationship = GetSingleParentRelationship(person);

        if (singleRelationship is null)
        {
            var relationship = new Relationship() {
                PersonId = person.Id, Children = new List<string>() { child.Id.ToString() }
            };
            _dynasty.Relationships.Add(relationship);
            return relationship;
        }

        singleRelationship.Children.Add(child.Id.ToString());

        return singleRelationship;
    }

    public Relationship? FindRelationship(Dynasty dynasty, Person person, Person partner)
    {
        return dynasty.Relationships.FirstOrDefault(r =>
            r.PartnerId.Equals(partner.Id) && r.PersonId.Equals(person.Id) ||
            r.PartnerId.Equals(person.Id) || r.PersonId.Equals(partner.Id));
    }

    public IEnumerable<Relationship> FindRelationships(Person person)
    {
        return _dynasty.Relationships.Where(r => r.PersonId.Equals(person.Id) || r.PartnerId.Equals(person.Id));
    }

    public Relationship? GetSingleParentRelationship(Person person)
    {
        return _dynasty.Relationships.FirstOrDefault(r => r.PersonId.Equals(person.Id) && r.PartnerId is null);
    }
}