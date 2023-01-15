using Esfer.API.Domains.Shared.Domain;

namespace Esfer.API.Domains.Games.Domain.Entities;

public class Comment : Entity
{
    public int Id { get; set; }
    public Guid AccountId { get; set; }
    public string Value { get; set; }
    public int? ReplyingTo { get; set; }
    public DateTime CreatedAt { get; set; }

    public Comment()
    {

    }
}
