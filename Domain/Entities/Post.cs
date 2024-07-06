using Domain.Common;
using Domain.Common.Models;
using Domain.Entities.Photos;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Post : Entity
{
    public string Header { get; private set; }
    public string Text { get; private set; }
    public Guid AuthorId { get; private set; }
    public User Author { get; private set; }
    public IReadOnlyList<PostPhoto> Photos => _photos;
    private readonly List<PostPhoto> _photos = [];

    public Post()
    {
        
    }
    private Post(string header, string text, Guid authorId)
    {
        Header = header;
        Text = text;
        AuthorId = authorId;
    }

    public static Result<Post> Create(string header, string text, Guid authorId)
    {
        if (header.IsEmpty())
            return Errors.General.ValueIsRequired(nameof(header));
        if (text.IsEmpty())
            return Errors.General.ValueIsRequired(nameof(text));
        return new Post(header, text, authorId);
    }
}