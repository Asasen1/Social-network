using System.Security.Claims;

namespace Domain.Common;

public static class Permissions
{
    public static class UserPost
    {
        public const string Read = "posts.read";
        public const string Create = "myposts.create";
        public const string Update = "myposts.update";
        public const string Delete = "myposts.delete";
    }

    public static class Post
    {
        public const string Read = "posts.read";
        public const string Delete = "posts.delete";
    }
}