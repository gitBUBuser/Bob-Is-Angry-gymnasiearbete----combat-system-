using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    Transform[] positions;

    [SerializeField]
    float speed;

    int currentIndex;
    Rigidbody2D rb;
    public Vector2 Velocity { get; set; }

    List<Rigidbody2D> hookedObjects;
    private void Start()
    {
        currentIndex = 0;
        rb = GetComponent<Rigidbody2D>();
        hookedObjects = new List<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Move();
        IndexCheck();
    }

    void IndexCheck()
    {
        if(Vector2.Distance(transform.position,positions[currentIndex].position) < 0.4f)
        {
            if (currentIndex != positions.Length - 1)
                currentIndex++;
            else
                currentIndex = 0;
        }
    }

    void Move()
    {
        Vector2 direction = positions[currentIndex].position - transform.position;
        direction.Normalize();
        Velocity = (direction * speed);
        rb.MovePosition((Vector2)transform.position + Velocity * Time.fixedDeltaTime);
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            collision.transform.parent = transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.transform.parent = null;
        }
    }
}
