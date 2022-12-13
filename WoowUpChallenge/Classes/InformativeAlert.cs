
namespace WoowUpChallenge
{
    public class InformativeAlert : Alert
    {
        public InformativeAlert(string alertName, string topicName, string description, DateTime? expiresAt)
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
