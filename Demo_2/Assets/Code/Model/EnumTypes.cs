namespace ColorChessModel
{
    public enum EnumTypes
    {
        GameManager,
    }

    public enum TypeEvent
    {
        ClickCell
    }

    public enum CellType
    {
        Empty,
        Paint,
        Dark,
        Block
    }

    public enum FigureType
    {
        Empty,
        Pawn,
        King,
        Bishop,
        Castle,
        Horse,
        Queen,
    }

    public enum CornerType
    {
        Empty = 0,
        DownLeft = 1,
        DownRight = 2,
        UpRight = 3,
        UpLeft = 4,
    }

    public enum PlayerType
    {
        Human,
        AI,
        Online
    }

    public enum ColorType
    {
        Red,
        Blue,
        Yellow,
        Green,
        Purple
    }
}