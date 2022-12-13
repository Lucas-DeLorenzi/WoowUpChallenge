using WoowUpChallenge;

var systemManager = new AlertSystemManager();
var user1 = new User("Lucas");
var user2 = new User("Mario");
var user3 = new User("Andrea");
var user4 = new User("Maria");
var user5 = new User("Leandro");

//Subscribe users to alert system
Console.WriteLine("");

systemManager.Subscribe(user1);
systemManager.Subscribe(user2);
systemManager.Subscribe(user3);
systemManager.Subscribe(user4);
systemManager.Subscribe(user5);

//Create topics
Console.WriteLine("");

systemManager.CreateNewTopic("Topic 1");
systemManager.CreateNewTopic("Topic 2");
//Subscribe users to a topic
Console.WriteLine("");

user1.SubscribeToATopic(systemManager, "Topic 1");
user2.SubscribeToATopic(systemManager, "Topic 1");
user3.SubscribeToATopic(systemManager, "Topic 2");

//Create informative alert and notify subscribed users to a topic
Console.WriteLine("");

systemManager.CreateInformativeAlert("Informative alert 1", "Topic 1", "This is an informative alert for the topic 1", null);
systemManager.CreateInformativeAlert("Informative alert 2", "Topic 1", "This is an informative alert for the topic 1", new DateTime(2022, 12, 28));
systemManager.CreateInformativeAlert("Informative alert 3", "Topic 2", "This is an informative alert for the topic 2", new DateTime(2022, 12, 25));

//Create urgent alert and notify all subscribed users
Console.WriteLine("");

systemManager.CreateUrgentAlert("Urgent alert 1", "Topic 1", "This is an urgent alert for the topic 1", null);
systemManager.CreateUrgentAlert("Urgent alert 2", "Topic 2", "This is an urgent alert for the topic 2", new DateTime(2023,01,02));

//Mark alert as read for some users
Console.WriteLine("");

user1.MarkAlertAsRead(systemManager, "Informative alert 1");
user1.MarkAlertAsRead(systemManager, "Informative alert 2");
user2.MarkAlertAsRead(systemManager, "Urgent alert 1");
user3.MarkAlertAsRead(systemManager, "Urgent alert 1");

//Notify users about active informative and urgent alerts
Console.WriteLine("");

systemManager.NotifyAboutActiveUrgentAlerts();
systemManager.NotifyAboutActiveInformativeAlerts();

//Subscribe user to a topic
Console.WriteLine("");

user4.SubscribeToATopic(systemManager, "Topic 2");
user5.SubscribeToATopic(systemManager, "Topic 2");

//Create single user alert and notify to user
Console.WriteLine("");

systemManager.CreateSingleUserAlert("Single user alert 1", "Topic 2", "This is a single user alert for the topic 2", null, user4);
systemManager.CreateSingleUserAlert("Single user alert 2", "Topic 2", "This is a single user alert for the topic 2", null, user5);
systemManager.CreateSingleUserAlert("Single user alert 3", "Topic 2", "This is a single user alert for the topic 2", null, user5);

//Mark alert as read
Console.WriteLine("");

user5.MarkAlertAsRead(systemManager, "Single user alert 3");

//Notify user about single user alert
Console.WriteLine("");

systemManager.NotifyAboutSActiveSingleUserAlert("Single user alert 3", "Topic 2");
systemManager.NotifyAboutSActiveSingleUserAlert("Single user alert 2", "Topic 2");
systemManager.NotifyAboutSActiveSingleUserAlert("Single user alert 1", "Topic 2");





