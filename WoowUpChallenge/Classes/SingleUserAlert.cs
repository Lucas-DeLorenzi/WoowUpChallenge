
namespace WoowUpChallenge
{
    public class SingleUserAlert : Alert
    {
        public SingleUserAlert(string alertName, string topicName, string description, DateTime? expiresAt, ISubscriber user)
        {
            User = ((User)user);
            AlertName = alertName;
            TopicName = topicName;
            Description = description;
            CreatedAt = DateTime.Now;
            ExpiresAt = expiresAt;
            AlreadyReadBy = new List<User>();
        }
    }
}
