using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    Transform playerT;
    Vector2 currentPosition;
    Vector2 velocity;
    Vector3 wantedOffset = new Vector3(0, 0, -10);
    float smoothTime = 40;

    private void Start()
    {
        playerT = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        currentPosition = Vector2.SmoothDamp(currentPosition, playerT.position, ref velocity, smoothTime * Time.deltaTime);    
        transform.position = (Vector3)currentPosition + wantedOffset;
        
    }
}
