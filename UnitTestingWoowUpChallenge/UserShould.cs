using WoowUpChallenge;

namespace UnitTestingWoowUpChallenge
{
    public class UserShould
    {
        [Fact]
        public void MarkAlertAsRead()
        {
            var systemManager = new AlertSystemManager();
            var user = new User("Lucas");
            systemManager.Subscribe(user);
            systemManager.CreateNewTopic("Topic 1");
            systemManager.Subscribe(user, "Topic 1");
            systemManager.CreateInformativeAlert("Informative alert 1", "Topic 1", "This is an informative alert for topic 1", null);
            systemManager.CreateSingleUserAlert("Single user alert 1", "Topic 1", "This is an single user alert for topic 1", null, user);
            systemManager.CreateUrgentAlert("Urgent alert 1", "Topic 1", "This is an urgent alert for topic 1", null);

            user.MarkAlertAsRead(systemManager, "Informative alert 1");
            user.MarkAlertAsRead(systemManager, "Single user alert 1");
            user.MarkAlertAsRead(systemManager, "Urgent alert 1");

            Assert.Contains(user.Name, systemManager.UrgentAlerts[0].AlreadyReadBy[0].Name);
            Assert.Contains(user.Name, systemManager.Topics[0].InformativeAlerts[0].AlreadyReadBy[0].Name);
            Assert.Contains(user.Name, systemManager.Topics[0].SingleUserAlerts[0].AlreadyReadBy[0].Name);

        }
    }
}
