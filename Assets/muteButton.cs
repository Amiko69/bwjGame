using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class muteButton : MonoBehaviour
{
    public bool muted = false;

    void Start()
    {

    }
    public void muteUnmute()
    {
        muted = !muted;



        if (muted)
        {
            AudioListener.volume = 0;
        }

        if (muted == false)
        {
            AudioListener.volume = 1;
        }



    }
}
