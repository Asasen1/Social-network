using Domain.Common.Models;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Post : Entity
{
    public string Header { get; private set; }
    public string Description { get; private set; }
    public FullName Author { get; private set; }
    public IReadOnlyList<Photo> Photos => _photos;
    private readonly List<Photo> _photos = [];
}