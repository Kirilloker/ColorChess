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

    public void StartRegistration()
    {
        if ((loginInp.text.Length <= 2) || (passwordInp.text.Length <= 2)) return;

        //if (AddUser(loginInp.text, passwordInp.text) == true) 
        if (true)
        {
            ImageWarning.SetActive(false);

            BinarySerializer binarySerializer = new BinarySerializer();
            binarySerializer.SetDefaultData();
            binarySerializer.GetData()["login"] = loginInp.text;
            binarySerializer.GetData()["password"] = passwordInp.text;
            binarySerializer.SaveData();

            RegistrationMenu.SetActive(false);

            cameraController.SwitchCamera(CameraViewType.noteMenu);
        }
        else 
        {
            Debug.Log("не получилось зарегистрироваться грусть печаль");
            ImageWarning.SetActive(true);
        }


    }
}


