using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayer : MonoBehaviour
{
    PlayerMovement pm;
    Transform playerT;
    private void Awake()
    {
        playerT = GameObject.FindGameObjectWithTag("Player").transform;
        pm = GameObject.FindObjectOfType<PlayerMovement>();
    }


    private void LateUpdate()
    {
        //Previous follow


        //Smooth follow
        print(pm.isSwitching);
        if (pm.isSwitching == false)
        {

            // if (playerT)
            // {
            //     Vector3 smoothPosition = Vector3.Lerp(transform.position, playerT.position + playerT.right, 0.9f * Time.fixedDeltaTime);
            //     transform.position = new Vector3(smoothPosition.x, smoothPosition.y, -1);
            // }
            Vector3 temp = new Vector3(playerT.position.x, playerT.position.y, transform.position.z);
            transform.position = temp;

            //Rotation after player
            transform.rotation = playerT.rotation;
        }
    }
}
