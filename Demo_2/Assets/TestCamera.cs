using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamera : MonoBehaviour
{
    public Quaternion x;
    
    void Update()
    {
        //Debug.Log("===" + transform.rotation);
        //transform.rotation = Quaternion.AngleAxis(-90, Vector3.right);
        //Debug.Log("======" + x + " ====" + transform.rotation);
    }
}
