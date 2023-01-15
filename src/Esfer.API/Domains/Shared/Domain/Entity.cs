namespace Esfer.API.Domains.Shared.Domain;

public abstract class Entity
{
    public DateTime CreatedAt { get; protected set; }
    public DateTime? UpdatedAt { get; protected set; }
}
