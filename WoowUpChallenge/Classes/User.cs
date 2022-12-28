
namespace WoowUpChallenge
{
    public class User
    {
        private string _name { get; set; }
        private List<UserAlert> _alerts { get; set; }
        private List<Topic> _topicPreferences { get; set; }

        public string Name { get { return _name; } }
        public List<UserAlert> Alerts { get { return _alerts; } }
        public List<Topic> TopicPreferences { get { return _topicPreferences; } }


        public User(string name)
        {
            _name = name;
            _alerts = new List<UserAlert>();
            _topicPreferences = new List<Topic>();
        }

        public void SubscribeToTopic(Topic topic)
        {
            try
            {
                if (topic.UsersSubscribed.Contains(this))
                {
                    throw new Exception($"User {this.Name} is already subscribed to topic {topic.TopicName}");
                }
                else
                {
                    TopicPreferences.Add(topic);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //public void Unsubscribe(Topic topic)
        //{
        //    //Check if user already exist -> throw error / do nothing
        //    if (topic.UsersSubscribed.Contains(this))
        //    {
        //        TopicPreferences.Remove(topic);
        //    }
        //    else
        //    {
        //        throw new Exception($"User {this.Name} is not subscribed to topic {topic.TopicName}");
        //    }
        //}                                

        public void ReceiveAlert(UserAlert alert)
        {
            try
            {
                if (alert == null) throw new Exception("Alert is not exist");
                Alerts.Add(alert);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SeeAlerts()
        {
            Console.WriteLine($"You have {Alerts.Count} alerts. There are: {String.Join(", ", Alerts.Select(x => x.AlertName))}");
        }

        public void MarkAlertAsRead(string alertName, IManager manager)
        {
            try
            {
                var alert = _alerts.FirstOrDefault(alert => alert.AlertName == alertName);
                if (alert != null)
                {
                    alert.IsRead = true;
                    manager.ReceiveUserReadNotice(this, alertName);
                }
                else
                {
                    throw new Exception($"Alert with the given alert name: {alertName} not found");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
