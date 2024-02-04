using NUnit.Framework;
using ColorChessModel;

namespace UnitTest
{
    [TestFixture]
    public class TestPosition
    {
        [Test]
        public void Position_Equal() 
        {
            Position position_1 = new Position(10, 10);
            Position position_2 = new Position(10, 10);

            Assert.That(position_1, Is.EqualTo(position_2));
        }

    }
}
