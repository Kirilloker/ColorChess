using ColorChessModel;
using TMPro;
using UnityEngine;

public class Registration : MonoBehaviour
{
    [SerializeField]
    GameObject RegistrationMenu;
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

    Server server;

    public void Start()
    {
        server = Server.Instance;
    }

    public async void StartRegistration()
    {
        if (
            (loginInp.text.Length <= 2) || (passwordInp.text.Length <= 2)
            ||
            (loginInp.text.Length >= 20) || (passwordInp.text.Length >= 40)
            ) 
            return;

        if (await server.TryRegistry(loginInp.text, passwordInp.text) == true)
        {
            ImageWarning.SetActive(false);

            BinarySerializer binarySerializer = new BinarySerializer();
            binarySerializer.SetDefaultData();
            binarySerializer.GetData()["login"] = loginInp.text;
            binarySerializer.GetData()["password"] = passwordInp.text;
            binarySerializer.SaveData();

            RegistrationMenu.SetActive(false);

            cameraController.SwitchCamera(CameraViewType.noteMenu);

            AutoSignIn.Authorization();
        }
        else 
        {
            Print.Log("�� ���������� ������������������ ������ ������");
            ImageWarning.SetActive(true);
        }
    }
}


