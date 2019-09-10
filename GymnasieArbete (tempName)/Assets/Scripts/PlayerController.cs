using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Entity , IMoving
{
    [SerializeField]
    LayerMask groundLayer;
    MovementState state;


    public float Speed { get; set; }
    public Vector2 Velocity { get; set; }
    public MovementState State { get { return state; } set { state = value; } }

    float jumpTimeCounter;
    float jumpTime = 0.25f;
    bool wasGrounded;
    bool grounded;
    bool isJumping;
    
    public enum MovementState
    {
        Airborne,
        G_Idle,
        G_Running,
    }



    protected override void Start()
    {
        base.Start();
        jumpTimeCounter = jumpTime;
        Speed = 6;
    }


    private void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        Velocity = new Vector2(x * Speed, Velocity.y);

        if (Input.GetButtonDown("Jump") && grounded)
        {
            isJumping = true;
            Velocity = new Vector2(Velocity.x, 8f);
        }

        
        if (Input.GetButton("Jump") && isJumping)
        {
            
            if(jumpTimeCounter > 0)
            {
                Velocity = new Vector2(Velocity.x, 8f);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
        else
        {
            isJumping = false;
        }

        if (grounded)
        {
            jumpTimeCounter = jumpTime;
        }

        if(Velocity.x == 0 && grounded)
            state = MovementState.G_Idle;
        else if (!grounded)
            state = MovementState.Airborne;
        else
            state = MovementState.G_Running;
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
