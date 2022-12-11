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
    [SerializeField]
    Server server;

    public async void StartRegistration()
    {
        if (
            (loginInp.text.Length <= 2) || (passwordInp.text.Length <= 2)
            ||
            (loginInp.text.Length >= 40) || (passwordInp.text.Length >= 40)
            ) 
            return;

        //if (AddUser(loginInp.text, passwordInp.text) == true) 
        if (await server.TryRegisry(loginInp.text, passwordInp.text) == true)
        //bool x = true;
        //if (x)
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
            Debug.Log("не получилось зарегистрироваться грусть печаль");
            ImageWarning.SetActive(true);
        }


    }
}


