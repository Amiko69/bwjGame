using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public static void startGame()
    {
        SceneManager.LoadScene(1);
    }

    public static void loadMenu()
    {
        SceneManager.LoadScene(0);
    }
}
