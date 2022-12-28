
namespace WoowUpChallenge
{
    public class UrgentAlert : Alert
    {

        public UrgentAlert(string alertName, string topicName, string description, DateTime? expiresAt = null) : base(alertName, topicName, description, expiresAt)
        {
            IsUrgent = true;
        }
    }
}
