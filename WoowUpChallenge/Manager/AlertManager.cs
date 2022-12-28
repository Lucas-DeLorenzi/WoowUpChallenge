
namespace WoowUpChallenge
{
    public class AlertManager : IManager
    {
        private List<User> _users;
        private List<Alert> _alerts;
        private List<Topic> _topics;

        public List<User> Users { get { return _users; } }
        public List<Alert> Alerts { get { return _alerts; } }
        public List<Topic> Topics { get { return _topics; } }


        public AlertManager()
        {
            _users = new List<User>();
            _alerts = new List<Alert>();
            _topics = new List<Topic>();

        }

        #region private
        private Topic? CheckTopicExist(string topicName)
        {
            try
            {
                return _topics.FirstOrDefault(topic => topic.TopicName == topicName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private User? CheckUserExist(string? userName)
        {
            try
            {
                return _users.FirstOrDefault(user => user.Name == userName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SendAlert(Alert alert)
        {
            try
            {
                if (alert.IsUrgent)
                {
                    SendUrgentAlert(alert);
                }
                else
                {
                    if (alert.User != null)
                    {
                        SendExclusiveUserAlert(alert);
                    }
                    else
                    {
                        SendInformativeAlert(alert);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SendUrgentAlert(Alert alert)
        {
            try
            {
                foreach (var user in _users)
                {
                    user.ReceiveAlert(new UserAlert(alert));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SendExclusiveUserAlert(Alert alert)
        {
            try
            {
                var user = _users.First(user => user == alert.User);
                user.ReceiveAlert(new UserAlert(alert));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SendInformativeAlert(Alert alert)
        {
            try
            {
                foreach (var user in _users)
                {
                    if (user.TopicPreferences.Any(topic => topic.TopicName == alert.TopicName))
                    {
                        user.ReceiveAlert(new UserAlert(alert));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region public
        public void NewUser(string userName)
        {
            try
            {
                if (CheckUserExist(userName) != null) throw new Exception($"A user already exists with the given user name: {userName}");

                _users.Add(ContentCreator.CreateNewUser(userName));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void NewTopic(string topicName)
        {
            try
            {
                if (CheckTopicExist(topicName) != null) throw new Exception($"A topic already exists with the given topic name: {topicName}");

                _topics.Add(ContentCreator.CreateNewTopic(topicName));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SubscribeUserToTopic(string userName, string topicName)
        {
            try
            {
                var userSelected = CheckUserExist(userName);
                var topicSelected = CheckTopicExist(topicName);

                if (userSelected == null) throw new Exception($"User with the given user name: {userName} not found");
                if (topicSelected == null) throw new Exception($"Topic with the given topic name: {topicName} not found");

                userSelected.SubscribeToTopic(topicSelected);
                topicSelected.UsersSubscribed.Add(userSelected);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void NewInformativeAlert(string alertName, string topicName, string description, DateTime? expiresAt = null)
        {
            try
            {
                if (CheckTopicExist(topicName) == null) throw new Exception($"Topic with the given topic name: {topicName} not found");

                var newInformativeAlert = ContentCreator.CreateInformativeAlert(alertName, topicName, description, expiresAt);
                _alerts.Add(newInformativeAlert);
                SendAlert(newInformativeAlert);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void NewExclusiveUserAlert(string alertName, string topicName, string description, string userName, DateTime? expiresAt = null)
        {
            try
            {
                var topic = CheckTopicExist(topicName);
                var user = CheckUserExist(userName);

                if (topic == null) throw new Exception($"Topic with the given topic name: {topicName} not found");
                if (user == null) throw new Exception($"User with the given user name: {userName} not found");

                var newExlusiveUserAlert = ContentCreator.CreateInformativeAlert(alertName, topicName, description, expiresAt, user);
                _alerts.Add(newExlusiveUserAlert);
                SendAlert(newExlusiveUserAlert);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void NewUrgentAlert(string alertName, string topicName, string description, DateTime? expiresAt = null)
        {
            try
            {
                if (CheckTopicExist(topicName) == null) throw new Exception($"Topic with the given topic name: {topicName} not found");

                var newUrgentAlert = ContentCreator.CreateUrgentAlert(alertName, topicName, description, expiresAt);
                _alerts.Add(newUrgentAlert);
                SendAlert(newUrgentAlert);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ReceiveUserReadNotice(User user, string alertName)
        {
            try
            {
                if (user == null) throw new Exception("The user does not exist");

                Alert alert = _alerts.First(alert => alert.AlertName == alertName);
                alert.AlreadyReadBy.Add(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<UserAlert> GetUnreadAndUnexpiredUserAlerts(string userName)
        {
            try
            {
                var userSelected = CheckUserExist(userName);

                if (userSelected == null) throw new Exception($"User with the given user name: {userName} not found");

                List<UserAlert> unreadAndUnexpiredUserAlerts = new List<UserAlert>();
                unreadAndUnexpiredUserAlerts.AddRange(userSelected.Alerts.Where(alert => !alert.IsRead && (alert.ExpiresAt > DateTime.Now || alert.ExpiresAt == null)));
                return unreadAndUnexpiredUserAlerts.OrderByDescending(alert => alert.CreatedAt).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Alert> GetSortedAndUnexpiredTopicAlerts(string topicName)
        {
            try
            {
                List<Alert> unexpiredTopicAlerts = new List<Alert>();
                var selectedTopic = CheckTopicExist(topicName);
                if (selectedTopic != null)
                {
                    unexpiredTopicAlerts.AddRange(_alerts.Where(alert => alert.TopicName == topicName && alert.ExpiresAt > DateTime.Now || alert.ExpiresAt == null));
                    return unexpiredTopicAlerts.OrderByDescending(alert => alert.CreatedAt).ToList();
                }
                else
                {
                    throw new Exception($"No topic found with the given topic name: {topicName}");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        // Added this method to get a list of unexpired exclusive user alert sorted alphabetically and also sorted by topic
        // That is the reason for the AlertsQuery class
        public List<AlertsQuery> GetSortedAndUnexpiredExclusiveUserAlerts(User user)
        {
            try
            {
                List<AlertsQuery> unexpiredSingleUserAlerts = new List<AlertsQuery>();
                foreach (var topic in _topics)
                {
                    var topicQuery = new AlertsQuery();

                    topicQuery.Topic = topic;
                    topicQuery.SortedAlertList.AddRange(_alerts.Where(alert => alert.User == user && (alert.ExpiresAt > DateTime.Now || alert.ExpiresAt == null)));
                    topicQuery.SortedAlertList.OrderByDescending(alert => alert.CreatedAt).ToList();
                    unexpiredSingleUserAlerts.Add(topicQuery);
                }
                return unexpiredSingleUserAlerts;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }

}