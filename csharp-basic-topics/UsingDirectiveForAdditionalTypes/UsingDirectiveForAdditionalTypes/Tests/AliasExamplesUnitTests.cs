using UsingDirectiveForAdditionalTypes;

namespace Tests
{
    [TestClass]
    public class AliasExamplesUnitTests
    {
        [TestMethod]
        public void GivenAListOfCodeMazeArticles_WhenListIsRetrieved_ThenListNotEmpty()
        {
            var codeMazeArticles = AliasExamples.GetArticles();

            Assert.IsNotNull(codeMazeArticles);
            Assert.IsTrue(codeMazeArticles.Any());
        }
    }
}