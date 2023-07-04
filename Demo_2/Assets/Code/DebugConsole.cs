using TMPro;
using UnityEngine; 
using UnityEngine.UI; 

namespace ColorChessModel
{
    public class DebugConsole : MonoBehaviour
    {
        [SerializeField]
        TMP_Text TMP_Text;
        public static void Print(string str)
        {
            Debug.Log(str);
            System.Console.WriteLine(str);
        }

        public void PrintUI(string str) 
        {
            TMP_Text.text += str;
        }

    }
}