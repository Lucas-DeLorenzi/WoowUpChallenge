using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoowUpChallenge
{
    public class UrgentAlert : Alert
    {
        public UrgentAlert(string alertName, string topicName, string? description, DateTime? expiresAt)
        {
            AlertName = alertName;
            TopicName = topicName;
            Description = description;
            CreatedAt = DateTime.Now;
            ExpiresAt = expiresAt;
            AlreadyReadBy = new List<User>();
        }
    }
}
