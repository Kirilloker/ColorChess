using UnityEngine;

using UnityEngine.SceneManagement;

public class Preload : MonoBehaviour 
{
	void Start () 
    {
        SceneManager.LoadScene("SampleScene");
	}
}
