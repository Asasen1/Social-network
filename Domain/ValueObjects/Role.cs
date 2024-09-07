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
            Constants.Permissions.Post.Update,
            Constants.Permissions.Post.Delete,
            Constants.Permissions.Post.Create,
            Constants.Permissions.Post.Read,
            
            Constants.Permissions.User.Update,
            Constants.Permissions.User.Delete,
            Constants.Permissions.User.Create,
            Constants.Permissions.User.Read
        ]
    );
    public static readonly Role User = new(nameof(User).ToUpper(),
        [
            Constants.Permissions.Post.Update,
            Constants.Permissions.Post.Create,
            Constants.Permissions.Post.Read,
            
            Constants.Permissions.User.Update,
            Constants.Permissions.User.Delete,
            Constants.Permissions.User.Create,
            Constants.Permissions.User.Read
        ]
    );
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
    }
}