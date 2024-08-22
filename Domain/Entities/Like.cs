using Domain.Agregates;
using Domain.Common.Models;

namespace Domain.Entities;

public class Like : Entity
{
    public User User { get; private set; }
    public DateTime LikeDate { get; private set; }

    private Like()
    {
    }
    private Like(User user)
    {
        User = user;
        LikeDate = DateTime.Now;
    }
}