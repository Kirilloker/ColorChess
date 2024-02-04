using ColorChessModel;
using UnityEngine;

namespace Assets.Code
{
    public class UnityLogger : MonoBehaviour
    {
        private void Awake()
        {
            Print.printer = (string message) => Debug.Log(message);
        }
    }
}
