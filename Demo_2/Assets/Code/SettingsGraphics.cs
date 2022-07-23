using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsGraphics : MonoBehaviour
{
    [SerializeField]
    private Toggle SwitchGraphics;
    [SerializeField]
    private Light GameDesk;
    [SerializeField]
    private Material BoarMaterial;
    [SerializeField]
    private CameraController cameraManager;

    private bool LowGraphics;

    private void Awake()
    {
        ChangeGraphics();
    }
    public void ChangeGraphics()
    {
        LowGraphics = SwitchGraphics.isOn;

        //SwitchShadow();
        SwitchSmoothness();
        //SwitchCameraSpeed();

        //cameraManager.Set_Camera_Change_Speed(0f);
    }

    public void SwitchShadow()
    {
        if (LowGraphics == true)
            GameDesk.shadows = LightShadows.None;
        else
            GameDesk.shadows = LightShadows.Soft;
    }

    public void SwitchSmoothness()
    {
        if (LowGraphics == true)
            BoarMaterial.SetFloat("_Glossiness", 0f);
        else
            BoarMaterial.SetFloat("_Glossiness", 0.5f);
    }

    public void SwitchCameraSpeed()
    {
        //if (LowGraphics == true)
        //    cameraManager.Set_Camera_Change_Speed(0f);
        //else
        //    cameraManager.Set_Camera_Change_Speed(2f);
    }
}

