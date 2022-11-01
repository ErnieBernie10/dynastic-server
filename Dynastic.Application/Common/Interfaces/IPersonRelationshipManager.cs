using Dynastic.Domain.Entities;

namespace Dynastic.Application.Common.Interfaces;

public interface IPersonRelationshipManager
{
    /// <summary>
    /// Pairs two persons together into a relationship and adds the relationship to the dynasty
    /// </summary>
    /// <param name="person"></param>
    /// <param name="partner"></param>
    /// <returns></returns>
    Relationship PairPartner(Person person, Person partner);

    /// <summary>
    /// Pairs two persons together into a relationship and adds a child. Then adds the relationship to the dynasty
    /// </summary>
    /// <param name="child"></param>
    /// <param name="person"></param>
    /// <param name="partner"></param>
    /// <returns></returns>
    Relationship AddChild(Person child, Person person, Person partner);

    /// <summary>
    /// Creates a single person relationship and adds the child to it
    /// </summary>
    /// <param name="child"></param>
    /// <param name="person"></param>
    /// <returns></returns>
    Relationship AddChild(Person child, Person person);
}