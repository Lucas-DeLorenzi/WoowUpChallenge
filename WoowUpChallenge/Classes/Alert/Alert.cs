
namespace WoowUpChallenge
{
    public class Alert
    {
        public string AlertName { get; set; }
        public string TopicName { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsUrgent { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public User? User { get; set; }
        public List<User>? AlreadyReadBy { get; set; }

        public Alert(string alertName, string topic, string? description , DateTime? expiresAt = null,User? user = null)
        {
            AlertName = alertName;
            TopicName = topic;
            Description = description;
            ExpiresAt = expiresAt;
            User= user;


            IsUrgent = false;
            CreatedAt = DateTime.Now;
            AlreadyReadBy = new List<User>();
        }
    }
}
