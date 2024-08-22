using Domain.Common;
using Domain.Common.Models;

namespace Domain.ValueObjects;

public class Role : ValueObject
{
    public string Name { get; }
    public string[] Permissions { get; }

    private Role(string name, string[] permissions)
    {
        Name = name;
        Permissions = permissions;
    }

    public static readonly Role Admin = new(nameof(Admin).ToUpper(),
        [
            Common.Permissions.Post.Update,
            Common.Permissions.Post.Delete,
            Common.Permissions.Post.Create,
            Common.Permissions.Post.Read,
            
            Common.Permissions.User.Update,
            Common.Permissions.User.Delete,
            Common.Permissions.User.Create,
            Common.Permissions.User.Read
        ]
    );
    public static readonly Role User = new(nameof(User).ToUpper(),
        [
            Common.Permissions.Post.Update,
            Common.Permissions.Post.Create,
            Common.Permissions.Post.Read,
            
            Common.Permissions.User.Update,
            Common.Permissions.User.Delete,
            Common.Permissions.User.Create,
            Common.Permissions.User.Read
        ]
    );
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
    }
}