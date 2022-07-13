using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollCamera : MonoBehaviour
{

    public Slider slider;
    public CameraManager cameraManager;
 
    public void Awake()
    {
        slider.value = 3.2f;
    }

    public void RotateCamers() 
    {
        cameraManager.Change_Camers_Rotation(slider.value);
    }
}
