using WoowUpChallenge;

var systemManager = new AlertManager();

systemManager.NewTopic("Topic 1");
systemManager.NewUser("Lucas");
systemManager.SubscribeUserToTopic("Lucas", "Topic 1");
systemManager.NewInformativeAlert("Alert 1", "Topic 1", "This is an informative alert for the Topic 1", new DateTime(2023, 01, 20));
systemManager.NewInformativeAlert("Alert 2", "Topic 1", "This is an informative alert for the Topic 1", new DateTime(2023, 01, 23));
systemManager.NewInformativeAlert("Alert 3", "Topic 1", "This is an informative alert for the Topic 1", new DateTime(2023, 01, 14));
systemManager.NewExclusiveUserAlert("Alert 4", "Topic 1", "This is a exclusive user alert for the Topic 1", "Lucas", DateTime.Now);

var user = systemManager.Users.First(user => user.Name == "Lucas");

user.SeeAlerts();