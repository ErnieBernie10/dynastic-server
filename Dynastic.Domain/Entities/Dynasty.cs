using Dynastic.Domain.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Dynastic.Domain.Entities;

public class Dynasty : Base, ISearchable
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string? Motto { get; set; }
    public List<Person> Members { get; set; } = new();
    public List<Relationship> Relationships { get; set; } = new();
    public CreationStep CreationStep { get; set; }
    public bool IsPrimary { get; set; } = false;
    public DynastyOwnershipProperties OwnershipProperties { get; set; }
    // TODO: Make this structured once agreed on model
    public JsonDocument? CoaConfiguration { get; set; }
}

public enum CreationStep
{
    BasicInfo,
    Coa,
    Review,
    Finalized
}

public class DynastyOwnershipProperties
{
    public string? OwnerUserId { get; set; }
    public List<string> Members { get; set; } = new();

}