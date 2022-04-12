using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private Vector2 length, startpos;
    public GameObject cam;
    public float parallaxEffect;

    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        startpos = transform.position;
        length = GetComponent<SpriteRenderer>().bounds.size;
    }

    void FixedUpdate()
    {
        Vector2 temp = cam.transform.position * (1 - parallaxEffect);
        Vector2 dist = cam.transform.position * parallaxEffect;

        transform.position = startpos + dist;

        if (temp.x > startpos.x + length.x) 
        {
            startpos.x += length.x;
        }
        else if (temp.x<startpos.x - length.x)
        {
            startpos.x -= length.x;
        }

        if (temp.y > startpos.y + length.y)
        {
            startpos.y += length.y;
        }
        else if (temp.y < startpos.y - length.y)
        {
            startpos.y -= length.y;
        }
    }
}
