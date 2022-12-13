using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoowUpChallenge
{
    public class User : ISubscriber
    {
        public string Name { get; set; }
        public User(string name)
        {
            Name = name;
        }

        public ISubscriber? Update(IPublisher publisher)
        {
            if (!publisher.TriggeredAlert.AlreadyReadBy.Contains(this))
            {
                Console.WriteLine($"The user {this.Name} has received an alert: {publisher.TriggeredAlert.Description}");
                return this;
            }
            else
            {
                return null;
            }
        }

        public void SubscribeToATopic(AlertSystemManager publisher, string topicName)
        {
            publisher.Subscribe(this, topicName);
        }

        public void MarkAlertAsRead(IPublisher publisher, string alertName)
        {
            if (publisher.MarkAlertAsRead(alertName, this))
            {
                Console.WriteLine($"User {this.Name} has marked the alert: {alertName} as read");
            }
            else
            {
                throw new Exception($"No alert found with the given alert name: {alertName}");
            };
        }
    }
}
