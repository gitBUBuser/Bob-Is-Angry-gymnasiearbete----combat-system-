using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera cam;
    BoxCollider2D cameraBox;
    Transform playerT;

   
    private void Start()
    {
        cam = GetComponent<Camera>();
        cameraBox = GetComponent<BoxCollider2D>();
        playerT = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        AspectRatioBoxChange();
        FollowPlayer();
    }

    void AspectRatioBoxChange()
    {
        float xRatio = (float)Screen.width / (float)Screen.height;

        float yRatio = (float)Screen.height / (float)Screen.width;

        float boxSizeX = (Mathf.Abs(cam.orthographicSize) * 2 * xRatio);

        float boxSizeY = boxSizeX * yRatio;

        cameraBox.size = new Vector2(boxSizeX, boxSizeY);
    }

void FollowPlayer()
    {
        if (GameObject.Find("Boundary"))
        {
            BoxCollider2D boundary = GameObject.Find("Boundary").GetComponent<BoxCollider2D>();
            transform.position = new Vector3(Mathf.Clamp(playerT.position.x, boundary.bounds.min.x + cameraBox.size.x / 2, boundary.bounds.max.x - cameraBox.size.x / 2),
                Mathf.Clamp(playerT.position.y, boundary.bounds.min.y + cameraBox.size.y / 2, boundary.bounds.max.y - cameraBox.size.y / 2),
                transform.position.z);
                
        }
    }
}
