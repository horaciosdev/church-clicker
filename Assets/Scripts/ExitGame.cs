using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public void Exit()
    {
        Application.Quit();
        ToastCreator.CreateToast("Game is exiting...", "top-center");
    }
}
