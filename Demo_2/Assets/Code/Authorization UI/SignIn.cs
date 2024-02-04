using ColorChessModel;
using System;
using TMPro;
using UnityEngine;

public class SignIn : MonoBehaviour
{
    [SerializeField]
    GameObject SigInMenu;
    [SerializeField]
    TMP_InputField loginInp;
    [SerializeField]
    TMP_InputField passwordInp;
    [SerializeField]
    CameraController cameraController;
    [SerializeField]
    GameObject ImageWarning;
    [SerializeReference]
    AutoSignIn AutoSignIn;
    [SerializeField]
    GameObject StartMenuDesktop;


    Server server;

    public void Start()
    {
        server = Server.Instance;
    }

    public async void StartSigIn()
    {
        if (
            (loginInp.text.Length <= 2) || (passwordInp.text.Length <= 2)
            ||
            (loginInp.text.Length >= 40) || (passwordInp.text.Length >= 40)
            )
            return;

        try 
        {
            if (await server.TryLoginIn(loginInp.text, passwordInp.text) == true)
            {
                ImageWarning.SetActive(false);

                BinarySerializer binarySerializer = new BinarySerializer();
                binarySerializer.SetDefaultData();
                binarySerializer.GetData()["login"] = loginInp.text;
                binarySerializer.GetData()["password"] = passwordInp.text;
                binarySerializer.SaveData();

                SigInMenu.SetActive(false);

                cameraController.SwitchCamera(CameraViewType.noteMenu);

                AutoSignIn.Authorization();
            }
            else 
            {
                Print.Log("�� ���������� ���������������� ������ ������");
                ImageWarning.SetActive(true);
            }
        }
        catch (Exception ex) 
        {
            Print.Log(ex.Message);
        }
    }
}
