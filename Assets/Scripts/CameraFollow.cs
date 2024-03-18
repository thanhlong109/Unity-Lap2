using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform playerPos;

    private void Update()
    {
        Vector3 pos = new Vector3(playerPos.position.x, playerPos.position.y,transform.position.z);
        transform.position = pos;
    }
    
   
}
