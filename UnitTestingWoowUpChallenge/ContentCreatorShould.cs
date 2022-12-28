using WoowUpChallenge;

namespace UnitTestingWoowUpChallenge
{
    public class ContentCreatorShould
    {
        [Fact]
        public void BeAbleToCreateNewUser()
        {
            var user = ContentCreator.CreateNewUser("Lucas");

            Assert.IsType<User>(user);
        }

        [Fact]
        public void BeAbleToCreateNewTopic()
        {
            var topic = ContentCreator.CreateNewTopic("Topic 1");

            Assert.IsType<Topic>(topic);
        }

        [Fact]
        public void BeAbleToCreateInformativeAlert()
        {
            var user = ContentCreator.CreateNewUser("Lucas");

            var informativeAlert = ContentCreator.CreateInformativeAlert("Alert 1", "Topic 1", "This is an informative alert for the Topic 1");
            var exclusiveUserAlert = ContentCreator.CreateInformativeAlert("Alert 1", "Topic 1", "This is an exclusive user alert for the Topic 1", null, user);

            Assert.IsType<Alert>(informativeAlert);
            Assert.IsType<Alert>(exclusiveUserAlert);
        }

        [Fact]
        public void BeAbleToCreateUrgentAlert()
        {
            var urgentAlert = ContentCreator.CreateUrgentAlert("Alert 1", "Topic 1", "This is an urgent alert for the Topic 1");

            Assert.IsType<UrgentAlert>(urgentAlert);
        }
    }
}
