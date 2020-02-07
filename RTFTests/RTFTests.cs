using Xunit;

namespace RTFTests
{
    public class RTFTests
    {
        [Fact]
        public void TestEnableTrackedChanges()
        {
            const string testFile = @"testFiles/trackedDisabled.rtf";

            Assert.True(RTF.RTF.EnableTrackedChanges(testFile));
        }
    }
}
