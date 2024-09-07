namespace Domain.Constants;

public static class Permissions
{
    public static class Post
    {
        public const string Read = "posts.read";
        public const string Create = "posts.create";
        public const string Update = "posts.update";
        public const string Delete = "posts.delete";
    }
    public static class User
    {
        public const string Read = "user.read";
        public const string Create = "user.create";
        public const string Update = "user.update";
        public const string Delete = "user.delete";
    }
}