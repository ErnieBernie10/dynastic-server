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
        if (person.Equals(partner))
        {
            throw new ArgumentException("Can't pair a person with itself");
        }

        var relationship = GetRelationship(person, partner);
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

    public Relationship? GetRelationship(Person? person, Person? partner)
    {
        return _dynasty.Relationships.FirstOrDefault(r =>
                   r.PartnerId.Equals(partner?.Id) && r.PersonId.Equals(person?.Id) ||
                   r.PartnerId.Equals(person?.Id) || r.PersonId.Equals(partner?.Id)) ??
               GetSingleParentRelationship((person ?? partner) ?? throw new InvalidOperationException());
    }

    public IEnumerable<Relationship> GetRelationships(Person person)
    {
        return _dynasty.Relationships.Where(r => r.PersonId.Equals(person.Id) || r.PartnerId.Equals(person.Id));
    }

    public Relationship? GetSingleParentRelationship(Person person)
    {
        return _dynasty.Relationships.FirstOrDefault(r => r.PersonId.Equals(person.Id) && r.PartnerId is null);
    }

    public void UpdatePersonParents(Person person, Person? newFather, Person? newMother)
    {
        if ((newMother is not null && newMother.Id.Equals(person.MotherId)) &&
            (newFather is not null && newFather.Id.Equals(person.FatherId)))
        {
            return;
        }

        var oldMother = _dynasty.Members.FirstOrDefault(m => m.Id.Equals(person.MotherId));
        var oldFather = _dynasty.Members.FirstOrDefault(m => m.Id.Equals(person.FatherId));

        RemoveChild(person, oldMother, oldFather);

        if (newMother is not null && newFather is not null)
        {
            AddChild(person, newFather, newMother);
        }
        else
        {
            AddChild(person, (newFather ?? newMother) ?? throw new InvalidOperationException());
        }

        person.MotherId = newMother?.Id ?? null;
        person.FatherId = newFather?.Id ?? null;
    }

    private bool RemoveChild(Person person, Person? oldMother, Person? oldFather)
    {
        person.MotherId = null;
        person.FatherId = null;

        var relationship = GetRelationship(oldMother, oldFather);

        return relationship?.Children.Remove(person.Id.ToString()) ?? false;
    }
}