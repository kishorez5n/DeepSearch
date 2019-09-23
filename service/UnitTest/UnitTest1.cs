using ClassifyTextTry.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class UnitTestGraphQuery
    {

        GraphQuery graphClient;

        [TestInitialize]
        public void TestInit()
        {
            this.graphClient = GraphQuery.GetInstance();
        }

        [TestMethod]
        public void TestGetCategories()
        {
            var allCat = this.graphClient.GetCategories();
            Assert.IsNotNull(allCat);
        }

        [TestMethod]
        public void TestGetTravelSubCategories()
        {
            var allCat = this.graphClient.GetSubCategories("Travel");
            Assert.IsNotNull(allCat);
        }

        [TestMethod]
        public void TestGetTravelLinks()
        {
            var links = this.graphClient.GetCategoryLinks("Travel");
            Assert.IsNotNull(links);
        }
    }
}
