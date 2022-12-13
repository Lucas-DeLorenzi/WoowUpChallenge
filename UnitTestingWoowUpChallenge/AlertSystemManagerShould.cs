
using WoowUpChallenge;

namespace UnitTestingWoowUpChallenge
{
    public class AlertSystemManagerShould
    {
        [Fact]
        public void SubscribeUser()
        {

            var systemManager = new AlertSystemManager();
            var user1 = new User("Lucas");
            var user2 = new User("Mario");

            systemManager.Subscribe(user1);
            systemManager.Subscribe(user2);

            Assert.True(systemManager.Users.Count == 2);
            Assert.Contains(user1, systemManager.Users);
            Assert.Contains(user2, systemManager.Users);
        }
        [Fact]
        public void ThrowErrorWhenUserIsAlreadySubscribed()
        {

            var systemManager = new AlertSystemManager();
            var user1 = new User("Lucas");

            systemManager.Subscribe(user1);

            Assert.Throws<Exception>(() => systemManager.Subscribe(user1));
        }

        [Fact]
        public void CreateTopics()
        {

            var systemManager = new AlertSystemManager();

            systemManager.CreateNewTopic("Topic 1");
            systemManager.CreateNewTopic("Topic 2");
            systemManager.CreateNewTopic("Topic 3");

            Assert.True(systemManager.Topics.Count == 3);
            Assert.Equal("Topic 1", systemManager.Topics[0].TopicName);
            Assert.Equal("Topic 2", systemManager.Topics[1].TopicName);
            Assert.Equal("Topic 3", systemManager.Topics[2].TopicName);
        }

        [Fact]
        public void ThrowErrorWhenTopicIsAlreadySubscribed()
        {
            var systemManager = new AlertSystemManager();

            systemManager.CreateNewTopic("Topic 1");

            Assert.Throws<Exception>(() => systemManager.CreateNewTopic("Topic 1"));
        }

        [Fact]
        public void AllowUsersToSubscribeToTopics()
        {
            var systemManager = new AlertSystemManager();
            var user1 = new User("Lucas");

            systemManager.Subscribe(user1);
            systemManager.CreateNewTopic("Topic 1");
            user1.SubscribeToATopic(systemManager, "Topic 1");

            Assert.Contains(user1, systemManager.Topics[0].Users);
        }
        [Fact]
        public void ThrowErrorWhenUserIsAlreadySubscribedToATopic()
        {
            var systemManager = new AlertSystemManager();
            var user1 = new User("Lucas");

            systemManager.Subscribe(user1);
            systemManager.CreateNewTopic("Topic 1");
            user1.SubscribeToATopic(systemManager, "Topic 1");

            Assert.Throws<Exception>(() => user1.SubscribeToATopic(systemManager, "Topic 1"));
        }
        [Fact]
        public void ThrowErrorWhenASystemAlertUnsubscribedUserWantsToSubscribeToATopic()
        {
            var systemManager = new AlertSystemManager();
            var user1 = new User("Lucas");

            systemManager.CreateNewTopic("Topic 1");

            Assert.Throws<Exception>(() => user1.SubscribeToATopic(systemManager, "Topic 1"));
        }

        [Fact]
        public void ThrowErrorWhenUserWantsToSubscribeToANonExistentTopic()
        {
            var systemManager = new AlertSystemManager();
            var user1 = new User("Lucas");

            systemManager.Subscribe(user1);
            systemManager.CreateNewTopic("Topic 1");

            Assert.Throws<Exception>(() => user1.SubscribeToATopic(systemManager, "Topic 2"));
        }

        [Fact]
        public void CreateAnInformativeAlert()
        {
            var systemManager = new AlertSystemManager();
            systemManager.CreateNewTopic("Topic 1");
            string alertName = "Informative alert 1";
            string topicName = "Topic 1";
            string description = "This is an informative alert for the Topic 1";
            DateTime? expiresAt = null;


            systemManager.CreateInformativeAlert(alertName, topicName, description, expiresAt);

            Assert.NotEmpty(systemManager.Topics[0].InformativeAlerts);
            Assert.True(systemManager.Topics[0].InformativeAlerts[0].TopicName == topicName);
            Assert.True(systemManager.Topics[0].InformativeAlerts[0].AlertName == alertName);
            Assert.True(systemManager.Topics[0].InformativeAlerts[0].Description == description);
            Assert.True(systemManager.Topics[0].InformativeAlerts[0].ExpiresAt == expiresAt);
        }

        [Fact]
        public void ThrowErrorWhenTopicNameIsNotFoundCreatingANewInformativeAlert()
        {
            var systemManager = new AlertSystemManager();
            systemManager.CreateNewTopic("Topic 1");
            string alertName = "Informative alert 1";
            string topicName = "Topic 4";
            string description = "This is an informative alert for the Topic 1";
            DateTime? expiresAt = null;

            Assert.Throws<Exception>(() => systemManager.CreateInformativeAlert(alertName, topicName, description, expiresAt));
        }

        [Fact]
        public void ImmediateAlertToUsersWhenNewInformativeAlertIsCreated()
        {
            var systemManager = new AlertSystemManager();
            var user1 = new User("Lucas");
            var user2 = new User("Mario");
            var user3 = new User("Andrea");
            string alertName = "Informative alert 1";
            string topicName = "Topic 1";
            string description = "This is an informative alert for the Topic 1";
            DateTime? expiresAt = null;
            systemManager.Subscribe(user1);
            systemManager.Subscribe(user2);
            systemManager.Subscribe(user3);
            systemManager.CreateNewTopic("Topic 1");
            systemManager.CreateNewTopic("Topic 2");
            user1.SubscribeToATopic(systemManager, "Topic 1");
            user2.SubscribeToATopic(systemManager, "Topic 1");
            user3.SubscribeToATopic(systemManager, "Topic 2");

            var usersAlerted1 = systemManager.CreateInformativeAlert(alertName, topicName, description, expiresAt);
            var usersAlerted2 = systemManager.CreateInformativeAlert("Informative alert 2", "Topic 2", "This is an informative alert for the Topic 2", null);

            Assert.Contains(user1, usersAlerted1);
            Assert.Contains(user2, usersAlerted1);
            Assert.Contains(user3, usersAlerted2);
            Assert.DoesNotContain(user3, usersAlerted1);
        }

        [Fact]
        public void SendActiveInformativeAlertsToUsersSubscribedToATopic()
        {
            var systemManager = new AlertSystemManager();
            var user1 = new User("Lucas");
            var user2 = new User("Mario");
            var user3 = new User("Andrea");
            var user4 = new User("Monica");
            string alertName = "Informative alert 1";
            string topicName = "Topic 1";
            string description = "This is an informative alert for the Topic 1";
            DateTime? expiresAt = null;
            systemManager.Subscribe(user1);
            systemManager.Subscribe(user2);
            systemManager.Subscribe(user3);
            systemManager.Subscribe(user4);
            systemManager.CreateNewTopic("Topic 1");
            systemManager.CreateNewTopic("Topic 2");
            systemManager.CreateNewTopic("Topic 3");
            user1.SubscribeToATopic(systemManager, "Topic 1");
            user2.SubscribeToATopic(systemManager, "Topic 1");
            user3.SubscribeToATopic(systemManager, "Topic 2");
            user4.SubscribeToATopic(systemManager, "Topic 3");
            systemManager.CreateInformativeAlert(alertName, topicName, description, expiresAt);
            systemManager.CreateInformativeAlert("Informative alert 2", "Topic 2", "This is an informative alert for the Topic 2", new DateTime(2023, 08, 12));
            systemManager.CreateInformativeAlert("Informative alert 3", "Topic 3", "This is an informative alert for the Topic 3", new DateTime(2022, 02, 02));

            var usersAlerted = systemManager.NotifyAboutActiveInformativeAlerts();

            Assert.Contains(user1, usersAlerted);
            Assert.Contains(user2, usersAlerted);
            Assert.Contains(user3, usersAlerted);
            Assert.DoesNotContain(user4, usersAlerted);
        }

        [Fact]
        public void CreateASingleUserAlert()
        {
            var systemManager = new AlertSystemManager();
            var user = new User("Lucas");
            systemManager.CreateNewTopic("Topic 1");
            systemManager.Subscribe(user);
            systemManager.Subscribe(user, "Topic 1");
            string alertName = "Single user alert 1";
            string topicName = "Topic 1";
            string description = "This is an single user alert for the Topic 1";
            DateTime? expiresAt = null;


            systemManager.CreateSingleUserAlert(alertName, topicName, description, expiresAt, user);

            Assert.NotEmpty(systemManager.Topics[0].SingleUserAlerts);
            Assert.True(systemManager.Topics[0].SingleUserAlerts[0].TopicName == topicName);
            Assert.True(systemManager.Topics[0].SingleUserAlerts[0].AlertName == alertName);
            Assert.True(systemManager.Topics[0].SingleUserAlerts[0].Description == description);
            Assert.True(systemManager.Topics[0].SingleUserAlerts[0].ExpiresAt == expiresAt);
            Assert.True(systemManager.Topics[0].SingleUserAlerts[0].User == user);
        }

        [Fact]
        public void ThrowErrorWhenUserNotFoundCreatingANewSingleUserAlert()
        {
            var systemManager = new AlertSystemManager();
            var user = new User("Lucas");
            systemManager.CreateNewTopic("Topic 1");
            string alertName = "Single user alert 1";
            string topicName = "Topic 1";
            string description = "This is an single user alert for the Topic 1";
            DateTime? expiresAt = null;

            Assert.Throws<Exception>(() => systemManager.CreateSingleUserAlert(alertName, topicName, description, expiresAt, user));
        }

        [Fact]
        public void ImmediateAlertToUserWhenNewSingleUserAlertIsCreated()
        {
            var systemManager = new AlertSystemManager();
            var user = new User("Lucas");
            systemManager.CreateNewTopic("Topic 1");
            systemManager.Subscribe(user);
            systemManager.Subscribe(user, "Topic 1");
            string alertName = "Single user alert 1";
            string topicName = "Topic 1";
            string description = "This is an single user alert for the Topic 1";
            DateTime? expiresAt = null;


            var alertedUser = systemManager.CreateSingleUserAlert(alertName, topicName, description, expiresAt, user);

            Assert.Equal(user, alertedUser);
        }

        [Fact]
        public void SendActiveSingleUserAlertToUser()
        {
            var systemManager = new AlertSystemManager();
            var user = new User("Lucas");
            systemManager.CreateNewTopic("Topic 1");
            systemManager.Subscribe(user);
            systemManager.Subscribe(user, "Topic 1");
            string alertName = "Single user alert 1";
            string topicName = "Topic 1";
            string description = "This is an single user alert for the Topic 1";
            DateTime? expiresAt = null;


            systemManager.CreateSingleUserAlert(alertName, topicName, description, new DateTime(2022, 03, 04), user);
            systemManager.CreateSingleUserAlert("Single user alert 2", topicName, description, expiresAt, user);

            var alertedUser1 = systemManager.NotifyAboutSActiveSingleUserAlert(alertName, topicName);
            var alertedUser2 = systemManager.NotifyAboutSActiveSingleUserAlert("Single user alert 2", topicName);

            Assert.NotEqual(user, alertedUser1);
            Assert.Equal(user, alertedUser2);
        }

        [Fact]
        public void CreateUrgentAlert()
        {
            var systemManager = new AlertSystemManager();
            systemManager.CreateNewTopic("Topic 1");
            string alertName = "Urgent alert 1";
            string topicName = "Topic 1";
            string description = "This is an urgent alert for the Topic 1";
            DateTime? expiresAt = null;


            systemManager.CreateUrgentAlert(alertName, topicName, description, expiresAt);

            Assert.NotEmpty(systemManager.UrgentAlerts);
            Assert.True(systemManager.UrgentAlerts[0].TopicName == topicName);
            Assert.True(systemManager.UrgentAlerts[0].AlertName == alertName);
            Assert.True(systemManager.UrgentAlerts[0].Description == description);
            Assert.True(systemManager.UrgentAlerts[0].ExpiresAt == expiresAt);
        }

        [Fact]
        public void ImmediateAlertToUserWhenNewUrgentAlertIsCreated()
        {

            var systemManager = new AlertSystemManager();
            var user1 = new User("Lucas");
            var user2 = new User("Mario");
            var user3 = new User("Andrea");
            var user4 = new User("Monica");
            systemManager.Subscribe(user1);
            systemManager.Subscribe(user2);
            systemManager.Subscribe(user3);
            systemManager.Subscribe(user4);
            systemManager.CreateNewTopic("Topic 1");
            string alertName = "Urgent alert 1";
            string topicName = "Topic 1";
            string description = "This is an urgent alert for the Topic 1";
            DateTime? expiresAt = null;


            var alertedUsers = systemManager.CreateUrgentAlert(alertName, topicName, description, expiresAt);

            Assert.True(systemManager.Topics[0].Users.Count == 0);
            Assert.Contains(user1, alertedUsers);
            Assert.Contains(user2, alertedUsers);
            Assert.Contains(user3, alertedUsers);
            Assert.Contains(user4, alertedUsers);
        }

        [Fact]
        public void SendActiveUrgentAlerts()
        {
            var systemManager = new AlertSystemManager();
            var user1 = new User("Lucas");
            var user2 = new User("Mario");
            systemManager.Subscribe(user1);
            systemManager.Subscribe(user2);
            systemManager.CreateNewTopic("Topic 1");
            string alertName = "Urgent alert 1";
            string topicName = "Topic 1";
            string description = "This is an urgent alert for the Topic 1";
            DateTime? expiresAt = null;
            systemManager.CreateUrgentAlert(alertName, topicName, description, new DateTime(2022, 04,12));
            systemManager.CreateUrgentAlert("Urgent alert 2", topicName, description, expiresAt);

            var notifiedAlerts = systemManager.NotifyAboutActiveUrgentAlerts();

            Assert.True(notifiedAlerts.Count != systemManager.UrgentAlerts.Count);
            Assert.Contains(notifiedAlerts[0], systemManager.UrgentAlerts);
            Assert.NotEqual(notifiedAlerts[0], systemManager.UrgentAlerts[0]);
        }

        [Fact]
        public void ReturnSortedAndUnexpiredSingleUserAlertsList()
        {
            var systemManager = new AlertSystemManager();
            var user1 = new User("Lucas");
            systemManager.CreateNewTopic("Topic 1");
            systemManager.CreateNewTopic("Topic 2");
            systemManager.Subscribe(user1);
            systemManager.Subscribe(user1, "Topic 1");
            systemManager.Subscribe(user1, "Topic 2");
            string alertName = "Single user alert 1";
            string topicName1 = "Topic 1";
            string topicName2 = "Topic 2";
            string description1 = "This is an single user alert for the Topic 1";
            string description2 = "This is an single user alert for the Topic 2";
            DateTime? expiresAt = null;


            systemManager.CreateSingleUserAlert(alertName, topicName1, description1, expiresAt, user1);
            systemManager.CreateSingleUserAlert("Single user alert 2", topicName1, description1, expiresAt, user1);
            systemManager.CreateSingleUserAlert("Single user alert 3", topicName2, description2, expiresAt, user1);
            systemManager.CreateSingleUserAlert("Single user alert 4", topicName1, description1, new DateTime(2022, 01, 02), user1);

            var sortedList = systemManager.GetSortedAndUnexpiredSingleUserAlerts(user1);

            Assert.True(sortedList.Count == 3);
            Assert.Equal("Single user alert 3", sortedList[0].AlertName);
            Assert.Equal("Single user alert 2", sortedList[1].AlertName);
            Assert.Equal("Single user alert 1", sortedList[2].AlertName);
            Assert.True(sortedList[0].CreatedAt > sortedList[1].CreatedAt && sortedList[1].CreatedAt  > sortedList[2].CreatedAt);
        }

        [Fact]
        public void ReturnSortedAndUnexpiredTopicAlertsList()
        {
            var systemManager = new AlertSystemManager();
            var user1 = new User("Lucas");
            var user2 = new User("Mario");
            var user3 = new User("Jorgelina");
            systemManager.CreateNewTopic("Topic 1");
            systemManager.CreateNewTopic("Topic 2");
            systemManager.Subscribe(user1);
            systemManager.Subscribe(user2);
            systemManager.Subscribe(user3);
            systemManager.Subscribe(user1, "Topic 1");
            systemManager.Subscribe(user2, "Topic 1");
            systemManager.Subscribe(user3, "Topic 2");
            string alertName = "Single user alert 1";
            string topicName1 = "Topic 1";
            string topicName2 = "Topic 2";
            string description1 = "This is an single user alert for the Topic 1";
            string description2 = "This is an single user alert for the Topic 2";
            DateTime? expiresAt = null;


            systemManager.CreateSingleUserAlert(alertName, topicName1, description1, expiresAt, user1);
            systemManager.CreateSingleUserAlert("Single user alert 2", topicName1, description1, expiresAt, user2);
            systemManager.CreateSingleUserAlert("Single user alert 3", topicName2, description2, expiresAt, user3);
            systemManager.CreateSingleUserAlert("Single user alert 4", topicName1, description1, new DateTime(2022, 01, 02), user1);
            systemManager.CreateInformativeAlert("Informative alert 1", topicName1, "This is an informative alert for topic 1", null);
            systemManager.CreateInformativeAlert("Informative alert 2", topicName1, "This is an informative alert for topic 1", null);
            systemManager.CreateInformativeAlert("Informative alert 3", topicName1, "This is an informative alert for topic 1", new DateTime(2022, 06, 12));

            var sortedList = systemManager.GetSortedAndUnexpiredTopicAlerts("Topic 1");

            Assert.True(sortedList.Count == 4);
            Assert.Equal("Informative alert 2", sortedList[0].AlertName);
            Assert.Equal("Informative alert 1", sortedList[1].AlertName);
            Assert.Equal("Single user alert 2", sortedList[2].AlertName);
            Assert.Equal("Single user alert 1", sortedList[3].AlertName);
            Assert.Equal(user1, sortedList[3].User);
            Assert.Equal(user2, sortedList[2].User);
            Assert.Null(sortedList[1].User);
            Assert.Null(sortedList[0].User);
        }
    }
}
