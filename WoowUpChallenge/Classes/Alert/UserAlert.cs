
namespace WoowUpChallenge
{
    public class UserAlert
    {
        public string AlertName { get; set; }
        public string TopicName { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Urgent { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public bool IsRead { get; set; }

        public UserAlert(Alert alert)
        {
            AlertName = alert.AlertName;
            TopicName = alert.TopicName;
            Description = alert.Description;
            Urgent = alert.IsUrgent;
            CreatedAt = alert.CreatedAt;
            ExpiresAt = alert.ExpiresAt;
            IsRead = false;

        }
    }
}
