
namespace WoowUpChallenge
{
    public class UrgentAlert : Alert
    {
        public UrgentAlert(string alertName, string topicName, string? description, DateTime? expiresAt)
        {
            AlertName = alertName;
            TopicName = topicName;
            Description = description;
            CreatedAt = DateTime.Now;
            ExpiresAt = expiresAt;
            AlreadyReadBy = new List<User>();
        }
    }
}
