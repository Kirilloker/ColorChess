using ColorChessModel;
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
    private CameraController cameraController;

    public void StartSigIn()
    {
        if ((loginInp.text.Length <= 2) || (passwordInp.text.Length <= 2)) return;

        // if (CheckUserCorrect(loginInp.text, passwordInp.text) == false) return;

        BinarySerializer binarySerializer = new BinarySerializer();
        binarySerializer.SetDefaultData();
        binarySerializer.GetData()["login"] = loginInp.text;
        binarySerializer.GetData()["password"] = passwordInp.text;
        binarySerializer.SaveData();

        SigInMenu.SetActive(false);

        cameraController.SwitchCamera(CameraViewType.noteMenu);
    }
}
