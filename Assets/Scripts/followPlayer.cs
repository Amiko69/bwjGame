using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayer : MonoBehaviour
{
    Transform playerT;
    private void Awake()
    {
        playerT = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        //Previous follow
        //Vector3 temp = new Vector3(playerT.position.x, playerT.position.y, transform.position.z);
        //transform.position = temp;

        //Smooth follow
        if (playerT)
        {
            Vector3 smoothPosition = Vector3.Lerp(transform.position, playerT.position + playerT.right, 0.9f * Time.fixedDeltaTime);
            transform.position = new Vector3(smoothPosition.x, smoothPosition.y, -1);
        }

        //Rotation after player
        transform.rotation = playerT.rotation;
    }
}
