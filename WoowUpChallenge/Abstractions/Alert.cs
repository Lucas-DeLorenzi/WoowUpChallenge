
namespace WoowUpChallenge
{
    public abstract class Alert
    {
        public User User { get; set; }
        public string AlertName { get; set; }
        public string TopicName { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public List<User>? AlreadyReadBy { get; set; }
    }
}
