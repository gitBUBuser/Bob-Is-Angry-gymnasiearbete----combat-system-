using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform playerT;
    Vector2 currentPosition;
    Vector3 velocity;
    Vector3 wantedOffset = new Vector3(0, 0, -10);
    float smoothTime = 20f;
    Camera camera;
    float currentX;
    float currentY;
    int width;
    int height;
    bool resetXPos;
    bool resetYpos;

    private void Start()
    {
        camera = GetComponent<Camera>();
        width = camera.pixelWidth;
        height = camera.pixelHeight;
        playerT = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if(camera.WorldToScreenPoint(playerT.position).x > width * 0.75f || camera.WorldToScreenPoint(playerT.position).x < width * 0.25f)
        {
            currentX = playerT.position.x;
        }

        if (camera.WorldToScreenPoint(playerT.position).y > height * 0.75f || camera.WorldToScreenPoint(playerT.position).y < height * 0.25f)
        {
            currentY = playerT.position.y;
        }

        if(playerT.GetComponent<PlayerController>().State == PlayerController.MovementState.G_Running || playerT.GetComponent<PlayerController>().State == PlayerController.MovementState.Airborne)
        {
            transform.position = Vector3.SmoothDamp(transform.position, new Vector2(currentX,currentY),ref velocity, smoothTime * Time.deltaTime) + wantedOffset;
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, playerT.position, ref velocity, smoothTime * Time.deltaTime) + wantedOffset;
        }
       
        
    }
}
