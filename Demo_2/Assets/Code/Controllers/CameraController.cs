using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using ColorChessModel;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    //Главная камера
    [SerializeField]
    private CinemachineBrain mainCamera;

    //Камеры
    [SerializeField]
    private List<CinemachineVirtualCamera> cinemachines;

    public CameraViewType viewCamera = CameraViewType.gameStart;

    // Изменение угла во время игры
    [SerializeField]
    private Slider slider;

    private CinemachineTrackedDolly trackInGame1;
    private CinemachineTrackedDolly trackInGame2;


    private void Start()
    {
        SwitchCamera();

        slider.value = 3.2f;
        mainCamera.m_DefaultBlend.m_Time = 2f;
        trackInGame1 = cinemachines[(int)CameraViewType.inGame1].GetCinemachineComponent<CinemachineTrackedDolly>();
        trackInGame2 = cinemachines[(int)CameraViewType.inGame2].GetCinemachineComponent<CinemachineTrackedDolly>();
    }

    private void SwitchCamera()
    {
        foreach (var cinemachine in cinemachines)
        {
            cinemachine.m_Priority = 1;
        }

        cinemachines[(int)viewCamera].m_Priority = 10;
    }

    public void SwitchCamera(CameraViewType _viewCamera)
    {
        viewCamera = _viewCamera;
        Invoke("SwitchCamera", 1f);
    }

    public void RotateCamers()
    {
        ChangeCamersRotation(slider.value);
    }

    public void ChangeCamersRotation(float angle)
    {
        // Почему-то в Awake не получается инициализировать trackInGame, поэтому тут вот такой костыль
        if (trackInGame1 == null && trackInGame2 == null)
        {
            cinemachines[(int)CameraViewType.inGame1].GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition = angle;
            cinemachines[(int)CameraViewType.inGame2].GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition = angle;

            trackInGame1 = cinemachines[(int)CameraViewType.inGame1].GetCinemachineComponent<CinemachineTrackedDolly>();
            trackInGame2 = cinemachines[(int)CameraViewType.inGame2].GetCinemachineComponent<CinemachineTrackedDolly>();
        }
        else
        {
            trackInGame1.m_PathPosition = angle;
            trackInGame2.m_PathPosition = angle;
        }
    }

    public void SetCameraSpeed(float speed)
    {
        mainCamera.m_DefaultBlend.m_Time = speed;
    }

    public float GetCameraSpeed()
    {
        return mainCamera.m_DefaultBlend.m_Time;
    }

    public void CameraToMenu()
    {
        // Нужна для UI кнопок
        SwitchCamera(CameraViewType.noteMenu);
    }

    public void CameraToDesktop()
    {
        SwitchCamera(CameraViewType.deskMenu);
    }
}

