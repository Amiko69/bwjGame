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
        Vector3 temp = new Vector3(playerT.position.x, playerT.position.y, transform.position.z);
        transform.position = temp;

        //Rotation after player
        transform.rotation = playerT.rotation;
    }
}
