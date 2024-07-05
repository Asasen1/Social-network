using Domain.Common.Models;
using Domain.Entities.Photos;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Post : Entity
{
    public string Header { get; private set; }
    public string Text { get; private set; }
    public FullName Author { get; private set; }
    public IReadOnlyList<Photo> Photos => _photos;
    private readonly List<Photo> _photos = [];
    

    public Post(Guid id, string header, string text, FullName author) : base(id)
    {
        Header = header;
        Text = text;
        Author = author;
    }
}