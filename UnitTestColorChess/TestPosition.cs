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
            // Организация
            Position position_2 = new Position(10, 10);
            Position position_3 = new Position(15, 15);

            // Действие
            bool pos_equal_pos2 = position == position_2; 
            bool pos_equal_pos3 = position != position_3; 

            // Утверждение
            Assert.IsTrue(pos_equal_pos2);
            Assert.IsFalse(pos_equal_pos3);
        }

        [Test]
        public void Position_ConvertString() 
        {
            string original = "X:10;   Y:10";

            StringAssert.Contains(original, position.ToString());
        }
    }
}
