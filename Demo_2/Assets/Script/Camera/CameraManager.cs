using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    //Главная камера
    public Cinemachine.CinemachineBrain main_camera;

    //Камеры
    public List<CinemachineVirtualCamera> cinemachines;


    private void Awake()
    {
        SwitchCamera(ViewCamera.gameStart);

        main_camera.m_DefaultBlend.m_Time = 2f;
    }

    public void SetEnumState(GetEnum g)
    {
        SwitchCamera(g.state);
    }

    public void SwitchCamera(ViewCamera viewCamera) 
    {
        for (int i = 0; i < cinemachines.Count; i++)
        {
            cinemachines[i].m_Priority = 1;
        }

        cinemachines[(int) viewCamera].m_Priority = 2;
    }

    public void Change_Camers_Rotation(float angle)
    {
        cinemachines[(int) ViewCamera.inGame1].GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition = angle;
        cinemachines[(int) ViewCamera.inGame2].GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition = angle;
    }

    //Изменение скорости перехода между камеры
    public void  Set_Camera_Change_Speed(float value)
    {
        if (ChangeSettingGraphics.LowGraphics == true) 
        {
            main_camera.m_DefaultBlend.m_Time = 0f;
        }
        else 
        {
            main_camera.m_DefaultBlend.m_Time = value;
        }
    }
}
