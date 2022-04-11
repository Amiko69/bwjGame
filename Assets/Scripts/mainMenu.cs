using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public GameObject creditCanvas;
    GameObject settingsCanvas;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void playButton()
    {
        SceneManager.LoadScene(0);
    }

    public void creditButton()
    {
        creditCanvas.SetActive(true);
    }

    public void Back()
    {
        creditCanvas.SetActive(false);
    }
}
