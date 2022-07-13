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
        Empty,
        UpLeft,
        UpRight,
        DownLeft,
        DownRight,
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