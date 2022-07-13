using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameNote : MonoBehaviour
{

    public void surrender()
    { 
        GameObject.FindWithTag("play_session_setting").GetComponent<Play_session_settings>().surrender();
    }
}
