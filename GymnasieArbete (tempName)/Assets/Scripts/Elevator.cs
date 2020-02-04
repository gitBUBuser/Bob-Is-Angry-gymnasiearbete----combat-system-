using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField]
    Transform newPos;

    bool shouldMove;
    float wantedY;
    float leaveTimer;
    [SerializeField]
    float leaveTime;
    float startY;
    Rigidbody2D rb;
    float yVelocity;

    [SerializeField]
    float speed;

    void Start()
    {
        startY = transform.position.y;
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float myY = transform.position.y;

        if (shouldMove)
        {
            leaveTimer = 0;
            wantedY = newPos.position.y;
        }
        else
        {
            leaveTimer += Time.fixedDeltaTime;
            if (leaveTimer > leaveTime)
            {
                wantedY = startY;
            }
        }

        if (Mathf.Abs(myY - wantedY) > 0.1f)
        {
            float distance = wantedY - myY;
            yVelocity = distance / Mathf.Abs(distance);
            yVelocity *= speed;
            rb.MovePosition((Vector2)transform.position + (new Vector2(0, yVelocity) * Time.fixedDeltaTime));
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Entity>())
        {
            shouldMove = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Entity>())
        {
            shouldMove = false;
        }
    }
}
