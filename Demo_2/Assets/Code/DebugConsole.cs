using UnityEngine; 

namespace ColorChessModel
{
    public static class DebugConsole
    {
        public static void Print(string str)
        {
            Debug.Log(str);
            System.Console.WriteLine(str);
        }

    }
}