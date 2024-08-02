namespace Domain.Common;

public static class Permissions
{
    public static class User
    {
        public const string Read = "posts.read";
        public const string Create = "posts.create";
        public const string Update = "posts.update";
        public const string Delete = "posts.delete";
    }
    public static class Admin
    {
        
    }
}