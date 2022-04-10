using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class music : MonoBehaviour
{
    AudioSource[] layers;

    void Start()
    {
        layers = GetComponents<AudioSource>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (layers[0].volume < .15f)
        {
            layers[0].volume += .0005f;
        }
        if (layers[1].volume < .15f)
        {
            layers[1].volume += .000008f;
        }
        if (layers[2].volume < .15f)
        {
            layers[2].volume += .000006f;
        }
        if (layers[3].volume < .15f)
        {
            layers[3].volume += .000004f;
        }
        if (layers[4].volume < .15f)
        {
            layers[4].volume += .000002f;
        }
    }
}
