using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DeleteTrackCamera : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachines;
    Transform NoneTransform;

    public void DeleteTrack()
    {
        cinemachines.m_LookAt = NoneTransform;
        cinemachines.m_Follow = NoneTransform;
    }


}
