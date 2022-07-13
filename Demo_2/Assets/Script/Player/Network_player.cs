using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class Network_player : Player
{

    public override void test_step(Figure _figure, Vector3 _new_position)
    {
        if (GameObject.Find("play_session").GetComponent<Play_session>().get_online_number() == GameObject.Find("play_session").GetComponent<Play_session>().get_number_player_step()) 
        {
            GameObject.Find("test server").GetComponent<Launcher>().SendMyStep(_figure.name, _new_position);
        }
    }
}
