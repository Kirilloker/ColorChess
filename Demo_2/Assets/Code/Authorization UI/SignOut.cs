using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignOut : MonoBehaviour
{
    [SerializeField]
    private CameraController cameraController;

    [SerializeField]
    GameObject SignInUI;
    public void SignOutAccount()
    {
        BinarySerializer binarySerializer = new BinarySerializer();
        binarySerializer.SetDefaultData();
        binarySerializer.GetData()["login"] = "";
        binarySerializer.GetData()["password"] = "";
        binarySerializer.SaveData();

        SignInUI.SetActive(true);

        cameraController.SwitchCamera(ColorChessModel.CameraViewType.deskMenu);
    }
}
