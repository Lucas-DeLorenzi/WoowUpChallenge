using WoowUpChallenge;

namespace UnitTestingWoowUpChallenge
{
    public class UserShould
    {
        [Fact]
        public void SubscribeToTopic()
        {
            var user = new User("Lucas");
            var topic = new Topic("Topic 1");

            user.SubscribeToTopic(topic);

            Assert.Contains(topic, user.TopicPreferences);
        }

        [Fact]
        public void ThrowExceptionWhenUserIsAlreadySubscribedForATopic()
        {
            var systemManager = new AlertManager();
            systemManager.NewTopic("Topic 1");
            systemManager.NewUser("Lucas");

            systemManager.SubscribeUserToTopic("Lucas", "Topic 1");

            Assert.Throws<Exception>(() => systemManager.SubscribeUserToTopic("Lucas", "Topic 1"));
        }

        [Fact]
        public void ReceiveAnAlert()
        {
            var user = new User("Lucas");
            var informativeAlert = new Alert("Alert 1", "Topic 1", "This is an informative alert for the Topic 1", new DateTime(2023, 01, 20));
            var userAlert = new UserAlert(informativeAlert);

            user.ReceiveAlert(userAlert);

            Assert.Contains(userAlert, user.Alerts);
        }

        [Fact]
        public void ThrowExceptionWhenReceivedAlertDoesNotExist()
        {
            var user = new User("Lucas");
            UserAlert? userAlert = null;

            Assert.Throws<Exception>(() => user.ReceiveAlert(userAlert));
        }

        [Fact]
        public void BeAbleToMarkAnAlertAsRead()
        {
            var alertSystem = new AlertManager();
            alertSystem.NewUser("Lucas");
            alertSystem.NewTopic("Topic 1");
            alertSystem.SubscribeUserToTopic("Lucas", "Topic 1");
            alertSystem.NewInformativeAlert("Alert 1", "Topic 1", "This is an informative alert for the Topic 1", new DateTime(2023, 01, 20));
            var user = alertSystem.Users[0];

            user.MarkAlertAsRead("Alert 1", alertSystem);

            Assert.True(user.Alerts.Any(alert => alert.AlertName == "Alert 1" && alert.IsRead));
        }

        [Fact]
        public void ThrowExceptionWhenTryingToFlagANonExistentAlert()
        {
            var alertSystem = new AlertManager();
            alertSystem.NewUser("Lucas");
            alertSystem.NewTopic("Topic 1");
            alertSystem.SubscribeUserToTopic("Lucas", "Topic 1");
            alertSystem.NewInformativeAlert("Alert 1", "Topic 1", "This is an informative alert for the Topic 1", new DateTime(2023, 01, 20));
            var user = alertSystem.Users[0];

            Assert.Throws<Exception>(() => user.MarkAlertAsRead("Alert 2", alertSystem));
        }
    }
}
