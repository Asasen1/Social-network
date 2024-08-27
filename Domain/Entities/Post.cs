using Domain.Common;
using Domain.Common.Models;
using Domain.Constraints;

namespace Domain.Entities;

public class Post : Entity
{
    public string Header { get; private set; }
    public string Text { get; private set; }
    public IReadOnlyList<PostPhoto> Photos => _photos;
    private readonly List<PostPhoto> _photos = [];
    public IReadOnlyList<Like> Likes => _likes;
    private readonly List<Like> _likes = [];
    public IReadOnlyList<Comment> Comments => _comments;
    private readonly List<Comment> _comments = [];
    private Post()
    {
        
    }
    private Post(string header, string text)
    {
        Header = header;
        Text = text;
    }

    public static Result<Post> Create(string header, string text)
    {
        if (header.IsEmpty())
            return Errors.General.ValueIsRequired(nameof(header));
        
        if (text.IsEmpty())
            return Errors.General.ValueIsRequired(nameof(text));
        
        if (header.Length > PostConstraints.MAX_HEADER_LENGTH ||
            header.Length < PostConstraints.MIN_HEADER_LENGTH)
            return Errors.General.InvalidLength(nameof(header));
        
        if (text.Length > PostConstraints.MAX_TEXT_LENGTH ||
            text.Length < PostConstraints.MIN_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid(nameof(text));
        
        return new Post(header, text);
    }
}