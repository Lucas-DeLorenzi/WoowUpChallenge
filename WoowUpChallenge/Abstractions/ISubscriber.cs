
namespace WoowUpChallenge
{
    public interface ISubscriber
    {
        ISubscriber? Update(IPublisher publisher);
        void MarkAlertAsRead(IPublisher publisher, string alertName);
        void SubscribeToATopic(AlertSystemManager publisher, string topicName);
    }
}
