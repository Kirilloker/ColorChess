using ColorChessModel;

namespace UnitTest
{
    [TestFixture]
    public class TestPosition
    {
        private Position position;

        [SetUp]
        public void Setup() 
        {
            position = new Position(10, 10);
        }

        [Test]
        public void Position_Equal() 
        {
            Position position_2 = new Position(10, 10);
            Position position_3 = new Position(15, 15);

            Assert.IsTrue(position == position_2);
            Assert.IsFalse(position == position_3);
            Assert.IsTrue(position != position_3);
        }

        [Test]
        public void Position_ConvertString() 
        {
            string original = "X:10;   Y:10";

            StringAssert.Contains(original, position.ToString());
        }
    }
}
