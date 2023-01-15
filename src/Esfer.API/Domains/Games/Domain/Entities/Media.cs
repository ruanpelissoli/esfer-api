using Esfer.API.Domains.Games.Domain.Enums;
using Esfer.API.Domains.Shared.Domain;

namespace Esfer.API.Domains.Games.Domain.Entities;

public class Media : Entity
{
    public int Id { get; set; }
    public string StorageUrl { get; set; }
    public EMediaType MediaType { get; set; }
}
