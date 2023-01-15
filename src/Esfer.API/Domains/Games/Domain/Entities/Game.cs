using Esfer.API.Domains.Games.Domain.ValueObjects;
using Esfer.API.Domains.Shared.Domain;

namespace Esfer.API.Domains.Games.Domain.Entities;

public class Game : Entity, IAggregateRoot
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public Price Price { get; private set; }
    public DateTime ReleaseDate { get; private set; }
    public IEnumerable<Media> Medias { get; private set; }
    public IEnumerable<GameFile> GameFiles { get; private set; }
    public IEnumerable<Comment> Comments { get; set; }

    protected Game() { }

    public Game(string title, string description, Price price, DateTime releaseDate)
    {
        Id = Guid.NewGuid();

        Title = title;
        Description = description;
        Price = price;
        ReleaseDate = releaseDate;

        Medias = new List<Media>();
        GameFiles = new List<GameFile>();
        Comments = new List<Comment>();
    }
}
