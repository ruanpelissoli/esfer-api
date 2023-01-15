using Esfer.API.Domains.Shared.Domain;

namespace Esfer.API.Domains.Games.Domain.Entities;

public class GameFile : Entity
{
    public int Id { get; set; }
    public string StorageUrl { get; set; }
    public string Version { get; set; }
    public DateTime ReleasedDate { get; set; }
}
