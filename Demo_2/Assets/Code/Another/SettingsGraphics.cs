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
    [SerializeField]
    private FigureController figureController;

    private bool LowGraphics;

    private void Awake()
    {
        ChangeGraphics();
    }

    public void ChangeGraphics()
    {
        LowGraphics = SwitchGraphics.isOn;

        SwitchShadow();
        SwitchSmoothness();
        SwitchCameraSpeed();
        SwitchFigureAnimation();
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
        if (LowGraphics == true)
            cameraManager.SetCameraSpeed(0f);
        else
            cameraManager.SetCameraSpeed(2f);
    }

    public void SwitchFigureAnimation()
    {
        figureController.LowGraphics = LowGraphics;
    }
}

