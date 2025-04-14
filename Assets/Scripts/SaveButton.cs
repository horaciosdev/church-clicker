using UnityEngine;

public class SaveButton : MonoBehaviour
{
    public GameObject saveManager;

    public void SaveGame()
    {
        saveManager.GetComponent<SaveManager>().SaveGame();
        ToastCreator.CreateToast("Jogo Salvo!", "bottom-left");
    }
}
