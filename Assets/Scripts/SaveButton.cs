using UnityEngine;

public class SaveButton : MonoBehaviour
{
    public GameObject saveManager;

    public void SaveGame()
    {
        saveManager.GetComponent<SaveManager>().SaveGame();
    }
}
