using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoowUpChallenge
{
    public interface IPublisher
    {
        public Alert? TriggeredAlert { get; set; }
        bool MarkAlertAsRead(string alertName, User user);
        void Subscribe(ISubscriber subscriber);
        List<ISubscriber> Notify();
    }
}
