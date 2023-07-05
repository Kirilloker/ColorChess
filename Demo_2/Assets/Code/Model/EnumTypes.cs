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
        DownLeft = 0,
        DownRight = 1,
        UpRight = 2,
        UpLeft = 3,
        Empty = 4,
    }

    public enum PlayerType
    {
        Human,
        AI,
        Online,
        AI2
    }

    public enum ColorType
    {
        Default,
        Red,
        Blue,
        Yellow,
        Green,
        Purple
    }

    public enum GameModeType
    {
       HumanTwo,
       HumanFour,
       AI,
       Network
    }

    public enum CameraViewType
    {
        gameStart = 0,
        noteMenu = 1,
        inGame1 = 2,
        inGame2 = 3,
        deskMenu = 4
    }

    public enum SoundType
    {
        Step,
        DarkCapture,
        ReverseStep,
        ReverseDarkCapture
    }
}