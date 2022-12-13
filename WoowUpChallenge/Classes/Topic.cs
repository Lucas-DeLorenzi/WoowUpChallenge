
namespace WoowUpChallenge
{
    public class Topic : IPublisher
    {
        public string TopicName { get; set; }
        private List<ISubscriber> _users;
        private List<InformativeAlert> _informativeAlerts;
        private List<SingleUserAlert> _singleUserAlerts;
        public Alert? TriggeredAlert { get; set; } = null;

        public Topic(string topicName)
        {
            TopicName = topicName;
            _users = new List<ISubscriber>();
            _informativeAlerts = new List<InformativeAlert>();
            _singleUserAlerts = new List<SingleUserAlert>();
        }

        public List<ISubscriber> Users { get { return _users; } }
        public List<InformativeAlert> InformativeAlerts { get { return _informativeAlerts; } }
        public List <SingleUserAlert> SingleUserAlerts { get { return _singleUserAlerts; } }    
        public List<ISubscriber> CreateInformativeAlert(string alertName, string description, DateTime? expiresAt)
        {
            InformativeAlert newInformativeAlert = new InformativeAlert(alertName, this.TopicName, description, expiresAt);
            _informativeAlerts.Add(newInformativeAlert);
            TriggeredAlert = newInformativeAlert;
            var usersAlerted = Notify();
            return usersAlerted;
        }

        public ISubscriber? CreateSingleUserAlert(string alertName, string description, DateTime? expiresAt, User user)
        {
            SingleUserAlert newSingleUserAlert = new SingleUserAlert(alertName, this.TopicName, description, expiresAt, user);
            _singleUserAlerts.Add(newSingleUserAlert);
            var alertedUser = NotifySingleUserAlert(newSingleUserAlert);
            return alertedUser;
        }
        public List<ISubscriber> Notify()
        {
            var usersAlerted = new List<ISubscriber>();
            foreach (var user in _users)
            {
                var result = user.Update(this);
                if (result != null)
                {
                    usersAlerted.Add(result);
                }
            }
            return usersAlerted;
        }

        public List<ISubscriber> NotifyAboutActiveInformativeAlerts()
        {
            var alertedUsers = new List<ISubscriber>();
            foreach (var alert in _informativeAlerts)
            {
                if (alert.ExpiresAt == null || alert.ExpiresAt > DateTime.Now)
                {
                    TriggeredAlert = alert;
                    var result = Notify();
                    alertedUsers.AddRange(result);
                }
            }
            return alertedUsers;
        }

        public ISubscriber? NotifySingleUserAlert(SingleUserAlert singleUserAlert)
        {
            ISubscriber? user = _users.FirstOrDefault(user => user == singleUserAlert.User);
            if (user != null)
            {
                TriggeredAlert = singleUserAlert;
                if (singleUserAlert.ExpiresAt == null || singleUserAlert.ExpiresAt > DateTime.Now)
                {
                    var alertedUser = user.Update(this);
                    return alertedUser;
                }
                return null;
            }
            else
            {
                throw new Exception($"User not found!");
            }
        }

        public void Subscribe(ISubscriber user)
        {
            if (!_users.Contains(user))
            {
                _users.Add(user);
                Console.WriteLine($"User {((User)user).Name} successfully suscribed to topic: {this.TopicName}");
            }
            else
            {
                throw new Exception($"User {((User)user).Name} has already subscribed for this topic");
            }
        }

        public bool MarkAlertAsRead(string alertName, User user)
        {
            Alert? informativeAlert = _informativeAlerts.FirstOrDefault(alert => alert.AlertName == alertName);
            if (informativeAlert != null)
            {
                informativeAlert.AlreadyReadBy.Add(user);
                return true;
            }
            else
            {
                Alert? singleUserAlert = _singleUserAlerts.FirstOrDefault(alert => alert.AlertName == alertName);
                if (singleUserAlert != null)
                {
                    singleUserAlert.AlreadyReadBy.Add(user);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public SingleUserAlert? GetSingleUserAlertByName(string singleUserAlertName)
        {
            return _singleUserAlerts.FirstOrDefault(alert => alert.AlertName == singleUserAlertName);
        }
    }
}
