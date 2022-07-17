using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using ColorChessModel;

public class CameraController : MonoBehaviour
{
    //Главная камера
    [SerializeField]
    private Cinemachine.CinemachineBrain main_camera;

    //Камеры
    [SerializeField]
    private List<CinemachineVirtualCamera> cinemachines;


    private void Awake()
    {
        SwitchCamera(CameraViewType.gameStart);

        main_camera.m_DefaultBlend.m_Time = 2f;
    }

    public void SwitchCamera(CameraViewType viewCamera)
    {
        for (int i = 0; i < cinemachines.Count; i++)
        {
            cinemachines[i].m_Priority = 1;
        }

        switch (viewCamera)
        {
            case CameraViewType.gameStart:
                cinemachines[0].m_Priority = 2;
                break;
            case CameraViewType.noteMenu:
                cinemachines[1].m_Priority = 2;
                break;
            case CameraViewType.inGame1:
                cinemachines[2].m_Priority = 2;
                break;
            case CameraViewType.inGame2:
                cinemachines[3].m_Priority = 2;
                break;
            case CameraViewType.deskMenu:
                cinemachines[4].m_Priority = 2;
                break;
            default:
                break;
        }
    }

    public void SwitchCameraWithDelay(CameraViewType viewCamera)
    {
        Invoke("SwitchCamera", 0.50f);
    }

    //public void Change_Cameras_Rotation(float angle)
    //{
    //    cinemachines[2].GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition = angle;
    //    cinemachines[3].GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition = angle;
    //}
    //Изменение скорости перехода между камеры
    //public void Set_Camera_Change_Speed(float value)
    //{
    //    if (ChangeSettingGraphics.LowGraphics == true)
    //    {
    //        main_camera.m_DefaultBlend.m_Time = 0f;
    //    }
    //    else
    //    {
    //        main_camera.m_DefaultBlend.m_Time = value;
    //    }
    //}
}

public enum CameraViewType
{
    gameStart,
    noteMenu,
    inGame1,
    inGame2,
    deskMenu
}