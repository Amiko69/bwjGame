using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;

    public void gameEnd()
    {
        Time.timeScale = 0.0f;
        player.SetActive(false);
    }
}
