
using WoowUpChallenge;

namespace UnitTestingWoowUpChallenge
{
    public class AlertManagerShould
    {
        [Fact]
        public void SubscribeNewUser()
        {

            var systemManager = new AlertManager();

            systemManager.NewUser("Lucas");

            Assert.True(systemManager.Users.Count == 1);
            Assert.True(systemManager.Users.Any(user => user.Name == "Lucas"));
        }

        [Fact]
        public void SubscribeNewTopic()
        {

            var systemManager = new AlertManager();

            systemManager.NewTopic("Topic 1");

            Assert.True(systemManager.Topics.Count == 1);
            Assert.True(systemManager.Topics.Any(topic => topic.TopicName == "Topic 1"));
        }

        [Fact]
        public void AllowUserToSubscribeToTopic()
        {
            var systemManager = new AlertManager();
            systemManager.NewTopic("Topic 1");
            systemManager.NewUser("Lucas");

            systemManager.SubscribeUserToTopic("Lucas", "Topic 1");

            Assert.True(systemManager.Topics.Any(topic => topic.TopicName == "Topic 1" && topic.UsersSubscribed.Any(user => user.Name == "Lucas")));
        }

        [Fact]
        public void AllowNewInformativeAlertToBeEnteredIntoTheSystem()
        {
            var systemManager = new AlertManager();
            systemManager.NewTopic("Topic 1");
            systemManager.NewUser("Lucas");
            systemManager.NewUser("Andrea");
            systemManager.SubscribeUserToTopic("Lucas", "Topic 1");

            systemManager.NewInformativeAlert("Alert 1", "Topic 1", "This is an informative alert for the Topic 1");

            Assert.True(systemManager.Alerts.Count == 1);
            Assert.True(systemManager.Alerts.Any(alert => alert.TopicName == "Topic 1" && alert.AlertName == "Alert 1"));
            Assert.True(systemManager.Users.Any(user => user.Name == "Lucas" && user.Alerts.Any(alert => alert.AlertName == "Alert 1")));
            Assert.True(systemManager.Users.Any(user => user.Name == "Andrea" && !user.Alerts.Any(alert => alert.AlertName == "Alert 1")));
        }

        [Fact]
        public void AllowNewExclusiveUserAlertToBeEnteredIntoTheSystem()
        {
            var systemManager = new AlertManager();
            systemManager.NewTopic("Topic 1");
            systemManager.NewUser("Lucas");
            systemManager.NewUser("Andrea");
            systemManager.SubscribeUserToTopic("Lucas", "Topic 1");

            systemManager.NewExclusiveUserAlert("Alert 1", "Topic 1", "This is a exclusive user alert for the Topic 1", "Andrea");

            Assert.True(systemManager.Alerts.Count == 1);
            Assert.True(systemManager.Alerts.Any(alert => alert.TopicName == "Topic 1" && alert.AlertName == "Alert 1" && alert.User.Name == "Andrea"));
            Assert.True(systemManager.Users.Any(user => user.Name == "Andrea" && user.Alerts.Any(alert => alert.AlertName == "Alert 1")));
            Assert.True(systemManager.Users.Any(user => user.Name == "Lucas" && !user.Alerts.Any(alert => alert.AlertName == "Alert 1")));
        }

        [Fact]
        public void AllowNewUrgentAlertToBeEnteredIntoTheSystem()
        {
            var systemManager = new AlertManager();
            systemManager.NewTopic("Topic 1");
            systemManager.NewUser("Lucas");
            systemManager.NewUser("Andrea");
            systemManager.SubscribeUserToTopic("Lucas", "Topic 1");

            systemManager.NewUrgentAlert("Alert 1", "Topic 1", "This is a urgent alert for the Topic 1");

            Assert.True(systemManager.Alerts.Count == 1);
            Assert.True(systemManager.Alerts.Any(alert => alert.TopicName == "Topic 1" && alert.AlertName == "Alert 1" && alert.IsUrgent));
            Assert.True(systemManager.Users.Any(user => user.Name == "Andrea" && user.Alerts.Any(alert => alert.AlertName == "Alert 1")));
            Assert.True(systemManager.Users.Any(user => user.Name == "Lucas" && user.Alerts.Any(alert => alert.AlertName == "Alert 1")));
        }

        [Fact]
        public void ReceiveUserReadNotice()
        {
            var systemManager = new AlertManager();
            systemManager.NewTopic("Topic 1");
            systemManager.NewUser("Lucas");
            systemManager.SubscribeUserToTopic("Lucas", "Topic 1");
            systemManager.NewInformativeAlert("Alert 1", "Topic 1", "This is an informative alert for the Topic 1");
            User user = systemManager.Users.First(user => user.Name == "Lucas");

            systemManager.ReceiveUserReadNotice(user, "Alert 1");

            Assert.True(systemManager.Alerts.Any(alert => alert.AlertName == "Alert 1" && alert.AlreadyReadBy.Any(reader => reader.Name == "Lucas")));
        }

        [Fact]
        public void ReturnASortedListOfUnreadAndUnexpiredUserAlerts()
        {
            var systemManager = new AlertManager();
            systemManager.NewTopic("Topic 1");
            systemManager.NewUser("Lucas");
            systemManager.SubscribeUserToTopic("Lucas", "Topic 1");
            systemManager.NewInformativeAlert("Alert 1", "Topic 1", "This is an informative alert for the Topic 1", new DateTime(2023, 01, 20));
            systemManager.NewInformativeAlert("Alert 2", "Topic 1", "This is an informative alert for the Topic 1", new DateTime(2023, 01, 23));
            systemManager.NewInformativeAlert("Alert 3", "Topic 1", "This is an informative alert for the Topic 1", new DateTime(2023, 01, 14));
            systemManager.NewExclusiveUserAlert("Alert 4", "Topic 1", "This is a exclusive user alert for the Topic 1", "Lucas", DateTime.Now);
            User user = systemManager.Users.First(user => user.Name == "Lucas");
            user.MarkAlertAsRead("Alert 2", systemManager);

            var alertList = systemManager.GetUnreadAndUnexpiredUserAlerts("Lucas");

            Assert.True(alertList.Count == 2);
            Assert.True(alertList[0].CreatedAt > alertList[1].CreatedAt);
            Assert.Equal("Alert 3", alertList[0].AlertName);
            Assert.Equal("Alert 1", alertList[1].AlertName);
        }

        [Fact]
        public void ReturnASortedListOfUnexpiredTopicAlerts()
        {
            var systemManager = new AlertManager();
            systemManager.NewTopic("Topic 1");
            systemManager.NewUser("Lucas");
            systemManager.SubscribeUserToTopic("Lucas", "Topic 1");
            systemManager.NewInformativeAlert("Alert 1", "Topic 1", "This is an informative alert for the Topic 1", new DateTime(2023, 01, 20));
            systemManager.NewInformativeAlert("Alert 2", "Topic 1", "This is an informative alert for the Topic 1", new DateTime(2023, 01, 23));
            systemManager.NewInformativeAlert("Alert 3", "Topic 1", "This is an informative alert for the Topic 1", new DateTime(2023, 01, 14));
            systemManager.NewExclusiveUserAlert("Alert 4", "Topic 1", "This is a exclusive user alert for the Topic 1", "Lucas", DateTime.Now);

            var alertList = systemManager.GetSortedAndUnexpiredTopicAlerts("Topic 1");

            Assert.True(alertList.Count == 3);
            Assert.True(alertList[0].CreatedAt > alertList[1].CreatedAt && alertList[1].CreatedAt > alertList[2].CreatedAt);
            Assert.Equal("Alert 3", alertList[0].AlertName);
            Assert.Equal("Alert 2", alertList[1].AlertName);
            Assert.Equal("Alert 1", alertList[2].AlertName);
        }

    }
}
