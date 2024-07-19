using Domain.Common.Models;

namespace Domain.Entities;

public class Comment : Entity
{
    public User User { get; private set; }
    public string Text { get; private set; }
    public DateTime CommentDate { get; private set; }

    private Comment(){}
    
    private Comment(User user, string text)
    {
        User = user;
        Text = text;
        CommentDate = DateTime.Now;
    }
}