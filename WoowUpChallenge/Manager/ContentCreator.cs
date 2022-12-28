namespace WoowUpChallenge
{
    public class ContentCreator
    {
        public static User CreateNewUser(string userName)
        {
            try
            {
                User newUser = new User(userName);
                Console.WriteLine($"Successfully created a new User: {userName}");
                return newUser;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Topic CreateNewTopic(string topicName)
        {
            try
            {
                Topic newTopic = new Topic(topicName);
                Console.WriteLine($"Successfully created a new Topic: {topicName}");
                return newTopic;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Alert CreateInformativeAlert(string alertName, string topicName, string description, DateTime? expiresAt = null, User? user = null)
        {
            try
            {
                Alert newInformativeAlert = new Alert(alertName, topicName, description, expiresAt, user);
                Console.WriteLine($"Successfully created a new Informative Alert: {alertName}");
                return newInformativeAlert;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Alert CreateUrgentAlert(string alertName, string topicName, string description, DateTime? expiresAt = null)
        {
            try
            {
                UrgentAlert newUrgentAlert = new UrgentAlert(alertName, topicName, description, expiresAt); ;
                Console.WriteLine($"Successfully created a new Urgent Alert: {alertName}");
                return newUrgentAlert;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}