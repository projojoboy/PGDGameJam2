using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public void CloseTheGame()
    {
        Debug.Log("closed the game");
        Application.Quit();
    }
}
