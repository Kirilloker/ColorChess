using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using ColorChessModel;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    //������� ������
    [SerializeField]
    private Cinemachine.CinemachineBrain mainCamera;

    //������
    [SerializeField]
    private List<CinemachineVirtualCamera> cinemachines;

    public CameraViewType viewCamera = CameraViewType.gameStart;

    // ��������� ���� �� ����� ����
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

    public void SwitchCamera()
    {
        foreach (var cinemachine in cinemachines)
        {
            cinemachine.m_Priority = 1;
        }

        cinemachines[(int)viewCamera].m_Priority = 10;
    }

    public void SwitchCameraWithDelay(CameraViewType _viewCamera)
    {
        viewCamera = _viewCamera;
        Invoke("SwitchCamera", 0.50f);
    }

    public void RotateCamers()
    {
        ChangeCamersRotation(slider.value);
    }
    public void ChangeCamersRotation(float angle)
    {
        // ������-�� � Awake �� ���������� ���������������� trackInGame, ������� ��� ��� ����� �������
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

    public void CameraToMenu()
    {
        SwitchCameraWithDelay(CameraViewType.noteMenu);
    }

}

