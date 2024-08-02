namespace AlsetTest.Models;

public class Journal
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string ContentURL { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
}