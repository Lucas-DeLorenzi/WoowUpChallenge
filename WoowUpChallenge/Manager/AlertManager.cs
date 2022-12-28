
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
            return _topics.FirstOrDefault(topic => topic.TopicName == topicName);
        }

        private User? CheckUserExist(string? userName)
        {
            return _users.FirstOrDefault(user => user.Name == userName);
        }

        private void SendAlert(Alert alert)
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

        private void SendUrgentAlert(Alert alert)
        {
            foreach (var user in _users)
            {
                user.ReceiveAlert(new UserAlert(alert));
            }
        }

        private void SendExclusiveUserAlert(Alert alert)
        {
            var user = _users.First(user => user == alert.User);
            user.ReceiveAlert(new UserAlert(alert));
        }

        private void SendInformativeAlert(Alert alert)
        {
            foreach (var user in _users)
            {
                if (user.TopicPreferences.Any(topic => topic.TopicName == alert.TopicName))
                {
                    user.ReceiveAlert(new UserAlert(alert));
                }
            }
        }
        #endregion


        #region public
        public void NewUser(string userName)
        {
            try
            {
                if (CheckUserExist(userName) == null)
                {
                    _users.Add(ContentCreator.CreateNewUser(userName));
                }
                else
                {
                    throw new Exception($"Already exists user with the given user name: {userName}");
                }

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
                if (CheckTopicExist(topicName) == null)
                {
                    _topics.Add(ContentCreator.CreateNewTopic(topicName));
                }
                else
                {
                    throw new Exception($"Already exists topic with the given topic name: {topicName}");
                }

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
                if (userSelected != null)
                {
                    if (topicSelected != null)
                    {
                        userSelected.SubscribeToTopic(topicSelected);
                        topicSelected.UsersSubscribed.Add(userSelected);
                    }
                    else
                    {
                        throw new Exception($"Topic with the given topic name: {topicName} not found");
                    }
                }
                else
                {
                    throw new Exception($"User with the given user name: {userName} not found");
                }
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
                if (CheckTopicExist(topicName) != null)
                {
                    var newInformativeAlert = ContentCreator.CreateInformativeAlert(alertName, topicName, description, expiresAt);
                    _alerts.Add(newInformativeAlert);
                    SendAlert(newInformativeAlert);
                }
                else
                {
                    throw new Exception($"Topic with the given topic name: {topicName} not found");
                }

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
                if (topic != null)
                {
                    if (user != null)
                    {
                        var newExlusiveUserAlert = ContentCreator.CreateInformativeAlert(alertName, topicName, description, expiresAt, user);
                        _alerts.Add(newExlusiveUserAlert);
                        SendAlert(newExlusiveUserAlert);
                    }
                    else
                    {
                        throw new Exception($"User with the given user name: {userName} not found");
                    }

                }
                else
                {
                    throw new Exception($"Topic with the given topic name: {topicName} not found");
                }

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
                if (CheckTopicExist(topicName) != null)
                {
                    var newUrgentAlert = ContentCreator.CreateUrgentAlert(alertName, topicName, description, expiresAt);
                    _alerts.Add(newUrgentAlert);
                    SendAlert(newUrgentAlert);
                }
                else
                {
                    throw new Exception($"Topic with the given topic name: {topicName} not found");
                }

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
                if (user == null) throw new Exception("User is not exist");
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
                if (userSelected != null)
                {
                    List<UserAlert> unreadAndUnexpiredUserAlerts = new List<UserAlert>();
                    unreadAndUnexpiredUserAlerts.AddRange(userSelected.Alerts.Where(alert => !alert.IsRead && (alert.ExpiresAt > DateTime.Now || alert.ExpiresAt == null)));
                    return unreadAndUnexpiredUserAlerts.OrderByDescending(alert => alert.CreatedAt).ToList();
                }
                else
                {
                    throw new Exception($"User with the given user name: {userName} not found");
                }

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

        //public List<AlertsQuery> GetSortedAndUnexpiredExclusiveUserAlerts(User user)
        //{
        //    try
        //    {
        //        List<AlertsQuery> unexpiredSingleUserAlerts = new List<AlertsQuery>();
        //        foreach (var topic in _topics)
        //        {
        //            var topicQuery = new AlertsQuery();

        //            topicQuery.Topic = topic;
        //            topicQuery.SortedAlertList.AddRange(_alerts.Where(alert => alert.User == user && (alert.ExpiresAt > DateTime.Now || alert.ExpiresAt == null)));
        //            topicQuery.SortedAlertList.OrderByDescending(alert => alert.CreatedAt).ToList();
        //            unexpiredSingleUserAlerts.Add(topicQuery);
        //        }
        //        return unexpiredSingleUserAlerts;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        #endregion
    }

}