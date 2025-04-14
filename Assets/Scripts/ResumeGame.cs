using UnityEngine;

public class ResumeGame : MonoBehaviour
{
    public void Resume()
    {
        if (GameObject.Find("ExitMenu") != null)
        {
            Destroy(GameObject.Find("ExitMenu"));
        }
    }
}
