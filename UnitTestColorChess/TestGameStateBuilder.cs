using ColorChessModel;

namespace UnitTest
{
    [TestFixture]
    public class TestGameStateBuilder
    {
        GameStateBuilder gameStateBuilder;
        [SetUp]
        public void Setup()
        {
            gameStateBuilder = new();
        }

        [Test]
        [TestCase("SetDefaultHotSeatGameState")]
        [TestCase("SetDefaultAIGameState")]
        [TestCase("SetDefaultOnlineGameState")]
        public void Test_CheckCorrectlyMapForDefaultMode(string methodName)
        {
            Map map = GetNewMap(methodName);

            // Количество Свободных клеток изначальна равна общей площади - количество фигур
            int free_cell = map.Width * map.Length - map.Players.Select(x => x.Figures.Count).Sum();

            Assert.IsNotNull(map);
            Assert.IsNotNull(map.Players);

            Assert.Multiple(() =>
            {
                // Всего два игрока
                Assert.That(map.PlayersCount, Is.EqualTo(2));
                // Сначала ходит первый игрок
                Assert.That(map.NumberPlayerStep, Is.EqualTo(0));
                // Игра не закончена с самого начала
                Assert.IsFalse(map.EndGame);
                // Лишние клетки не заняты
                Assert.That(map.CountEmptyCell, Is.EqualTo(free_cell));
            });
        }



        [Test]
        [TestCase("CustomMode")]
        [TestCase("SetDefaultHotSeatGameState")]
        [TestCase("SetDefaultAIGameState")]
        [TestCase("SetDefaultOnlineGameState")]
        public void Test_CheckUniquenessPlayerForDefaultAndCustomMode(string methodName)
        {
            Map map = GetNewMap(methodName);

            var players = map.Players;

            // Проверяем что Номера, угла и цвета уникальны у каждого игрока
            List<int> numberPlayers = players.Select(player => player.Number).ToList();
            List<CornerType> cornerPlayers = players.Select(player => player.Corner).ToList();
            List<ColorType> colorPlayers = players.Select(player => player.Color).ToList();

            Assert.IsTrue(IsUnique(numberPlayers));
            Assert.IsTrue(IsUnique(cornerPlayers));
            Assert.IsTrue(IsUnique(colorPlayers));
        }


        [Test]
        [TestCase("CustomMode")]
        [TestCase("SetDefaultHotSeatGameState")]
        [TestCase("SetDefaultAIGameState")]
        [TestCase("SetDefaultOnlineGameState")]
        public void Test_CheckCorrectStateCellBeforeBuilderForDefaultAndCustomMode(string methodName)
        {
            Map map = GetNewMap(methodName);

            for (int i = 0; i < map.Width; i++)
            {
                for (int j = 0; j < map.Length; j++)
                {
                    Cell cell = map.GetCell(i, j);

                    // Если на клетке никто не стоит, то все значения у неё должны быть по умолчанию
                    if (cell.Figure == null)
                    {
                        Assert.That(cell.NumberPlayer, Is.EqualTo(-1));
                        Assert.That(cell.FigureType, Is.EqualTo(FigureType.Empty));
                        Assert.That(cell.Type, Is.EqualTo(CellType.Empty));
                    }
                    // Если на клетке стоит фигура, то все значения должны отличаться от по умолчанию
                    else
                    {
                        Assert.That(cell.NumberPlayer, Is.Not.EqualTo(-1));
                        Assert.That(cell.FigureType, Is.Not.EqualTo(FigureType.Empty));
                        Assert.That(cell.Type, Is.Not.EqualTo(CellType.Empty));
                    }
                }
            }

            // Проверяем, что все позиции клеток уникальны
            var positions = new List<Position>();

            foreach (var cell in map.Cells)
                positions.Add(cell.Pos);

            Assert.IsTrue(IsUnique(positions));
        }


        [Test]
        [TestCase("CustomMode")]
        [TestCase("SetDefaultHotSeatGameState")]
        [TestCase("SetDefaultAIGameState")]
        [TestCase("SetDefaultOnlineGameState")]
        public void Test_CheckCorrectStatePlayerBeforeBuilderForDefaultAndCustomMode(string methodName)
        {
            Map map = GetNewMap(methodName);

            // Проверяем, поля у фигуры
            foreach (var player in map.Players)
            {
                foreach (var figure in player.Figures)
                {
                    Assert.That(figure.Player, Is.EqualTo(player));
                    Assert.That(figure.Type, Is.Not.EqualTo(FigureType.Empty));
                    Assert.IsTrue(figure.Require.Any());
                }
            }

            // Проверяем, что все позиции фигур уникальны
            var positions = new List<Position>();

            foreach (var player in map.Players)
                foreach (var figure in player.Figures)
                    positions.Add(figure.Pos);

            Assert.IsTrue(IsUnique(positions));
        }

        [Test]
        public void Test_CorrectTypePlayerForDefaultMode()
        {
            // Проверяем, что все типы игроков установлены правильно 

            gameStateBuilder.SetDefaultHotSeatGameState();
            var mapHotSeat = gameStateBuilder.CreateGameState();

            gameStateBuilder.SetDefaultAIGameState();
            var mapAI = gameStateBuilder.CreateGameState();

            gameStateBuilder.SetDefaultOnlineGameState();
            var mapOnline = gameStateBuilder.CreateGameState();

            Assert.Multiple(() =>
            {
                Assert.That(mapHotSeat.Players[0].Type, Is.EqualTo(PlayerType.Human));
                Assert.That(mapHotSeat.Players[1].Type, Is.EqualTo(PlayerType.Human));

                Assert.That(mapAI.Players[0].Type, Is.EqualTo(PlayerType.Human));
                Assert.That(mapAI.Players[1].Type, Is.EqualTo(PlayerType.AI));

                Assert.That(mapOnline.Players[0].Type, Is.EqualTo(PlayerType.Human));
                Assert.That(mapOnline.Players[1].Type, Is.EqualTo(PlayerType.Online));
            });
        }

        public void Test_createCustomMap() 
        {
            Map map = GetCustomMap();

            Assert.NotNull(map);

            // Проверяем, что все позиции фигур уникальны
            var positions = new List<Position>();

            foreach (var player in map.Players)
                foreach (var figure in player.Figures)
                    positions.Add(figure.Pos);


            Assert.Multiple(() =>
            {
                Assert.That(map.PlayersCount, Is.EqualTo(3));
                Assert.That(map.Width, Is.EqualTo(13));
                Assert.That(map.Length, Is.EqualTo(13));
                Assert.That(map.CountStep, Is.EqualTo(1));
                Assert.IsTrue(IsUnique(positions));
            });
        }


        private static bool IsUnique<T>(List<T> list)
        {
            return new HashSet<T>(list).Count == list.Count;
        }

        private Map GetNewMap(string methodName) 
        {
            if (methodName == "CustomMode") return GetCustomMap();

            var method = gameStateBuilder.GetType().GetMethod(methodName);
            method?.Invoke(gameStateBuilder, null);

            return gameStateBuilder.CreateGameState();
        }

        private Map GetCustomMap() 
        {
            int sizeMap = 13;
            PlayerType[] playerTypes = { PlayerType.Human, PlayerType.AI, PlayerType.Human };
            CornerType[] cornerTypes = { CornerType.DownLeft, CornerType.UpRight, CornerType.DownRight };
            ColorType[] colorTypes = { ColorType.Green, ColorType.Blue, ColorType.Red };

            gameStateBuilder.SetCustomGameState(sizeMap, playerTypes, cornerTypes, colorTypes);
            return gameStateBuilder.CreateGameState();
        }
    }
}
