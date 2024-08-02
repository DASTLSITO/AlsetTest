using Microsoft.AspNetCore.Identity;

namespace AlsetTest.Models;

public class User: IdentityUser
{
    public ICollection<Journal> Journals { get; set; }
    public ICollection<UserSubscriber> UserSuscribers { get; set; }
}