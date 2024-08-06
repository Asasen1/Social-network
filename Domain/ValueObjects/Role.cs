using Domain.Common;
using Domain.Common.Models;

namespace Domain.ValueObjects;

public class Role : ValueObject
{
    public string Name { get; private set; }
    public string[] Permissions { get; set; }

    private Role(string name, string[] permissions)
    {
        Name = name;
        Permissions = permissions;
    }

    public static readonly Role Admin = new("ADMIN",
        [
            Common.Permissions.Post.Read,
            Common.Permissions.Post.Delete,

            Common.Permissions.UserPost.Update,
            Common.Permissions.UserPost.Delete,
            Common.Permissions.UserPost.Create,
            Common.Permissions.UserPost.Read
        ]
    );
    public static readonly Role User = new("USER",
        [
            Common.Permissions.UserPost.Update,
            Common.Permissions.UserPost.Delete,
            Common.Permissions.UserPost.Create,
            Common.Permissions.UserPost.Read
        ]
    );
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
    }
}