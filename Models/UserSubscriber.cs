namespace AlsetTest.Models;

public class UserSubscriber
{
    public string UserId { get; set; }
    public User User { get; set; }
    public string SubscriberId { get; set; }
    public User Subscriber { get; set; }
}