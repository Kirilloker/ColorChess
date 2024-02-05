using ColorChessModel;

namespace UnitTest
{
    [TestFixture]
    public class TestJSONConverter
    {

        [Test]
        public void Test_JSONtoMap() 
        {
            GameStateBuilder gameStateBuilder = new();
            gameStateBuilder.SetDefaultHotSeatGameState();
            Map map = gameStateBuilder.CreateGameState();

            string map_JSON = JSONConverter.ConvertToJSON(map);
            Map map_unconvert = JSONConverter.ConvertJSONtoMap(map_JSON);

            Map changeMap = new Map(map);
            changeMap.Cells[5,5].NumberPlayer = 5;

            string changeMap_JSON = JSONConverter.ConvertToJSON(changeMap);
            Map changeMap_unconvert = JSONConverter.ConvertJSONtoMap(changeMap_JSON);

            Assert.Multiple(() =>
            {
                Assert.True(map.Equals(map_unconvert));
                Assert.False(map.Equals(changeMap));
            });
        }
    }
}
