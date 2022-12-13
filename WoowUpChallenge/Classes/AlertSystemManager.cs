using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoowUpChallenge
{
    public class AlertSystemManager : IPublisher
    {
        private List<ISubscriber> _users;
        private List<UrgentAlert> _urgentAlerts;
        private List<Topic> _topics;

        public AlertSystemManager()
        {
            _users = new List<ISubscriber>();
            _urgentAlerts = new List<UrgentAlert>();
            _topics = new List<Topic>();
            this.TriggeredAlert = TriggeredAlert;
        }
        public List<ISubscriber> Users { get { return _users; } }
        public List<UrgentAlert> UrgentAlerts { get { return _urgentAlerts; } }
        public List <Topic> Topics { get { return _topics; } }
        public Alert? TriggeredAlert { get; set; } = null;

        public void CreateNewTopic(string topicName)
        {
            if (_topics.FirstOrDefault(topic => topic.TopicName == topicName) == null)
            {
                _topics.Add(new Topic(topicName));
                Console.WriteLine($"Successfully created a new Topic: {topicName}");
            }
            else
            {
                throw new Exception($"Already exists topic with the given topic name: {topicName}");
            }
        }

        public List<ISubscriber> CreateInformativeAlert(string alertName, string topicName, string description, DateTime? expiresAt)
        {
            Topic? topic = _topics.FirstOrDefault(topic => topic.TopicName == topicName);
            if (topic != null)
            {
                var alertedUsers = topic.CreateInformativeAlert(alertName, description, expiresAt);
                return alertedUsers;
            }
            else
            {
                throw new Exception($"Topic with the given topic name: {topicName} not found");
            }
        }
        public List<ISubscriber> CreateUrgentAlert(string alertName, string topicName, string description, DateTime? expiresAt)
        {
            Topic? topic = _topics.FirstOrDefault(topic => topic.TopicName == topicName);
            if (topic != null)
            {
                UrgentAlert urgentAlert = new UrgentAlert(alertName, topicName, description, expiresAt);
                _urgentAlerts.Add(urgentAlert);
                TriggeredAlert = urgentAlert;
                var alertedUsers = Notify();
                return alertedUsers;
            }
            else
            {
                throw new Exception($"Topic with the given topic name: {topicName} not found");
            }
        }

        public ISubscriber? CreateSingleUserAlert(string alertName, string topicName, string description, DateTime? expiresAt, User user)
        {
            Topic? topic = _topics.FirstOrDefault(topic => topic.TopicName == topicName);
            if (topic != null)
            {
                var alertedUser = topic.CreateSingleUserAlert(alertName, description, expiresAt, user);
                return alertedUser;
            }
            else
            {
                throw new Exception($"Topic with the given topic name: {topicName} not found");
            }
        }

        public List<ISubscriber> NotifyAboutActiveInformativeAlerts()
        {
            var alertedUsers = new List<ISubscriber>();
            foreach (var topic in _topics)
            {
                var result = topic.NotifyAboutActiveInformativeAlerts();
                alertedUsers.AddRange(result);
            }
            return alertedUsers;
        }

        public List<UrgentAlert> NotifyAboutActiveUrgentAlerts()
        {
            var notifiedAlerts = new List<UrgentAlert>();
            foreach (var alert in _urgentAlerts)
            {
                if (alert.ExpiresAt == null || alert.ExpiresAt > DateTime.Now)
                {
                    TriggeredAlert = alert;
                    Notify();
                    notifiedAlerts.Add(alert);
                }
            }
            return notifiedAlerts;
        }

        public ISubscriber? NotifyAboutSActiveSingleUserAlert(string singleUserAlertName, string topicName)
        {

            Topic? topic = _topics.FirstOrDefault(topic => topic.TopicName == topicName);
            if (topic != null)
            {
                SingleUserAlert? alert = topic.GetSingleUserAlertByName(singleUserAlertName);
                if (alert != null)
                {
                    var alertedUser = topic.NotifySingleUserAlert(alert);
                    return alertedUser;
                }
                else
                {
                    throw new Exception($"Alert with the given alert name: {singleUserAlertName} not found");
                }
            }
            else
            {
                throw new Exception($"Topic with the given topic name: {singleUserAlertName} not found");
            }

        }
        public List<ISubscriber> Notify()
        {
            var usersAlerted = new List<ISubscriber>();
            foreach (var user in _users)
            {
                var result = user.Update(this);
                if(result != null)
                {
                    usersAlerted.Add(result);
                }
            }
            return usersAlerted;
        }

        public void Subscribe(ISubscriber user)
        {
            if (!_users.Contains(user))
            {
                _users.Add(user);
                Console.WriteLine($"User {((User)user).Name} successfully suscribed to the alert system");
            }
            else
            {
                throw new Exception($"The user {((User)user).Name} has already subscribed");
            }
        }

        public void Subscribe(ISubscriber user, string topicName)
        {
            if (!_users.Contains(user))
            {
                throw new Exception($"The user {((User)user).Name} has not already subscribed to the alert system");
            }
            else
            {
                Topic? topic = _topics.FirstOrDefault(topic => topic.TopicName == topicName);
                if (topic != null)
                {
                    topic.Subscribe(user);
                }
                else
                {
                    throw new Exception($"No topic found with the given topic name: {topicName}");
                }
            };
        }


        public bool MarkAlertAsRead(string alertName, User user)
        {
            Alert? alert = _urgentAlerts.FirstOrDefault(alert => alert.AlertName == alertName);
            if (alert != null)
            {
                alert.AlreadyReadBy.Add(user);
                return true;
            }
            else
            {
                Topic? topic = _topics.FirstOrDefault(topic => topic.MarkAlertAsRead(alertName, user) == true);
                if (topic != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<SingleUserAlert> GetSortedAndUnexpiredSingleUserAlerts(User user)
        {
            List<SingleUserAlert> unexpiredSingleUserAlerts = new List<SingleUserAlert>();
            foreach(var topic in _topics)
            {
                unexpiredSingleUserAlerts.AddRange(topic.SingleUserAlerts.Where(alert => alert.User == user && (alert.ExpiresAt > DateTime.Now || alert.ExpiresAt == null)));
            }
            return unexpiredSingleUserAlerts.OrderByDescending(alert => alert.CreatedAt).ToList();
        }

        public List<Alert> GetSortedAndUnexpiredTopicAlerts(string topicName)
        {
            List<Alert> unexpiredTopicAlerts = new List<Alert>();
            var selectedTopic = _topics.FirstOrDefault(topic => topic.TopicName == topicName);
            if(selectedTopic != null)
            {
                unexpiredTopicAlerts.AddRange(selectedTopic.SingleUserAlerts.Where(alert => alert.ExpiresAt > DateTime.Now || alert.ExpiresAt == null));
                unexpiredTopicAlerts.AddRange(selectedTopic.InformativeAlerts.Where(alert => alert.ExpiresAt > DateTime.Now || alert.ExpiresAt == null));
                return unexpiredTopicAlerts.OrderByDescending(alert => alert.CreatedAt).ToList();
            }
            else
            {
                throw new Exception($"No topic found with the given topic name: {topicName}");
            }

        }
    }
}
