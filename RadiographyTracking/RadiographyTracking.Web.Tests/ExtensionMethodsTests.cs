using NUnit.Framework;
using RadiographyTracking.Web.Utility;

namespace RadiographyTracking.Web.Tests
{
    [TestFixture]
    public class ExtensionMethodsTests
    {
        [TestCase("A1", "A", "1")]
        [TestCase("NSD", "NSD", "")]
        [TestCase("A1,C2", "A,C", "1,2")]
        [TestCase("CB2", "CB", "2")]
        [TestCase("A1,B1,CA1", "A,B,CA", "1,1,1")]
        [TestCase("B1,FM", "B,FM", "1")]
        [TestCase("ND,FM", "ND,FM", "")]
        [TestCase("F", "F", "")]
        public void SplitObservationTest(string input, string output1, string output2)
        {
            var result = input.SplitObservation();
            Assert.AreEqual(output1,result.Item1 ,string.Format("Expected output to be {0}, but is {1}", output1, result.Item1));
            Assert.AreEqual(output2, result.Item2 ,string.Format("Expected output to be {0}, but is {1}", output2, result.Item2));
        }
    }
}
