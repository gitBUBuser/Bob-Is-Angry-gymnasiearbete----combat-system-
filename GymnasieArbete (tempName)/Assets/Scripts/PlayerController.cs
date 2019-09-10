using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Entity , IMoving
{
    [SerializeField]
    LayerMask groundLayer;
    public float Speed { get; set; }
    public Vector2 Velocity { get; set; }

    bool wasGrounded;
    bool grounded;

    protected override void Start()
    {
        base.Start();
        Speed = 6;
    }


    private void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        Velocity = new Vector2(x * Speed, Velocity.y);

        if (Input.GetButtonDown("Jump"))
        {
            Velocity = new Vector2(Velocity.x, 10f);
        }
        
    }

    public void Move()
    {
        grounded = Grounded();
        if (!grounded)
        {
            Velocity -= new Vector2(0, Gravity * Time.deltaTime);
        }
        else if (!wasGrounded)
        {          
            Bounds bounds = Physics2D.OverlapArea(new Vector2(transform.position.x - 0.5f, transform.position.y - 0.5f),
            new Vector2(transform.position.x + 0.5f, transform.position.y - 0.51f + (Velocity.y * Time.fixedDeltaTime)), groundLayer).bounds;
            float yPos = bounds.center.y + bounds.extents.y + GetComponent<Collider2D>().bounds.extents.y;
            Velocity = new Vector2(Velocity.x, 0);
            RigidBody.position = new Vector2(RigidBody.position.x, yPos);
        }
        wasGrounded = grounded;

        RigidBody.MovePosition(RigidBody.position + Velocity * Time.fixedDeltaTime);
    }

    bool Grounded()
    {
        return Physics2D.OverlapArea(new Vector2(transform.position.x - 0.5f, transform.position.y - 0.5f),
            new Vector2(transform.position.x + 0.5f, transform.position.y - 0.51f + (Velocity.y * Time.fixedDeltaTime)), groundLayer);
    }

    

    private void FixedUpdate()
    {
        Move();
    }
}
