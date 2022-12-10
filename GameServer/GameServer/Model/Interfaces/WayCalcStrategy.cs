using System.Collections.Generic;
namespace ColorChessModel
{
    interface IWayCalcStrategy
    {
        public List<Cell> AllSteps(Map map, Figure figure);
        public List<Cell> Way(Map map, Position startPos, Position endPos, Figure figure);
    }
}