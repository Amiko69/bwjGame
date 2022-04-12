using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public static bool muted = false;
    public static AudioSource mainMenuMusic;
    public static void startGame()
    {
        SceneManager.LoadScene(1);
    }

    public static void loadMenu()
    {
        SceneManager.LoadScene(0);
    }
    public static void muteUnmute()
    {
        muted = !muted;

        if (mainMenuMusic != null)
        {
            if (muted)
            {
                mainMenuMusic.volume = 0;
            }

            if (muted == false)
            {
                mainMenuMusic.volume = 1.4f;
            }
        }
    }



}
