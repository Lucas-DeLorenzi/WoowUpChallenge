
namespace WoowUpChallenge
{
    public class Topic
    {
        public string TopicName { get; set; }
        public List<User> UsersSubscribed { get; set; }

        public Topic(string topicName)
        {
            TopicName = topicName;
            UsersSubscribed = new List<User>();
        }
    }
}
