namespace ColorChessModel
{
    public static class Print
    {
        public delegate void PrintFunc(string message);
#if DEBUG
        public static PrintFunc printer = (message) => Log(message);
#else
        public static PrintFunc printer;
#endif
        public static void Log(string message) => printer(message);
    }
}
