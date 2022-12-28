
namespace WoowUpChallenge
{
    public interface IManager
    {
        public void NewUser(string userName);
        public void NewTopic(string topicName);
        public void NewInformativeAlert(string alertName, string topicName, string description, DateTime? expiresAt = null);
        public void NewExclusiveUserAlert(string alertName, string topicName, string description, string userName, DateTime? expiresAt = null);
        public void NewUrgentAlert(string alertName, string topicName, string description, DateTime? expiresAt);
        public void ReceiveUserReadNotice(User user, string alertName);
        public List<UserAlert> GetUnreadAndUnexpiredUserAlerts(string userName);
        public List<Alert> GetSortedAndUnexpiredTopicAlerts(string topicName);
    }
}
