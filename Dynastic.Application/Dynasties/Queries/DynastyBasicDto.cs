using Dynastic.Domain.Entities;

namespace Dynastic.Application.Dynasties.Queries;

public class DynastyBasicDto : Base
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string? Motto { get; set; }
    public string CoaPath { get; set; } = default!;
}