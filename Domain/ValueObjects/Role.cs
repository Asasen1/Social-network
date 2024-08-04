using Domain.Common;
using Domain.Common.Models;

namespace Domain.ValueObjects;

public class Role : ValueObject
{
    public string Name { get; private set; }
    public string[] Permissons { get; set; }

    public Role(string name, string[] permissons)
    {
        Name = name;
        Permissons = permissons;
    }

    public static readonly Role Admin = new("ADMIN",
        [
            Permissions.Post.Read,
            Permissions.Post.Delete,

            Permissions.UserPost.Update,
            Permissions.UserPost.Delete,
            Permissions.UserPost.Create,
            Permissions.UserPost.Read
        ]
    );
    public static readonly Role User = new("USER",
        [
            Permissions.UserPost.Update,
            Permissions.UserPost.Delete,
            Permissions.UserPost.Create,
            Permissions.UserPost.Read
        ]
    );
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
    }
}