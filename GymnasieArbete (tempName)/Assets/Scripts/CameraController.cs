using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    BoxCollider2D cameraBox;
    Transform playerT;

    private void Start()
    {
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
        if (Camera.main.aspect >= (1.25f) && Camera.main.aspect < (1.3f))
        {
            cameraBox.size = new Vector2(18f, 14.3f);
        }

        if (Camera.main.aspect >= (1.3f) && Camera.main.aspect < (1.4f))
        {
            cameraBox.size = new Vector2(19.13f, 14.3f);
        }

        if (Camera.main.aspect >= (1.5f) && Camera.main.aspect < (1.6f))
        {
            cameraBox.size = new Vector2(21.6f, 14.3f);
        }

        if (Camera.main.aspect >= (1.6f) && Camera.main.aspect < (1.7f))
        {
            cameraBox.size = new Vector2(23f, 14.3f);
        }

        if (Camera.main.aspect >= (1.7f) && Camera.main.aspect < (1.8f))
        {
            cameraBox.size = new Vector2(25.4f, 14.3f);
        }
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
