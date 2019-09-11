using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Entity , IMoving
{
    [SerializeField]
    LayerMask groundLayer;
    [SerializeField]
    Transform feetTransform;

    MovementState state;


    public float Speed { get; set; }
    public Vector2 Velocity { get; set; }
    public MovementState State { get { return state; } set { state = value; } }

    float jumpTimeCounter;
    float jumpTime = 0.25f;
    float jumpForce = 3f;
    float slideTime = 0.65f;
    float slideForce = 15f;
    float slideTimeCounter;

    float xVel;

    bool wasGrounded;
    bool grounded;
    bool isJumping;

    Animator animator;
    CapsuleCollider2D worldCollider;

    public enum MovementState
    {
        Airborne,
        G_Idle,
        G_Running,
        G_Sliding
    }



    protected override void Start()
    {
        base.Start();
        worldCollider = GetComponent<CapsuleCollider2D>();
        xVel = 0;
        jumpTimeCounter = jumpTime;
        slideTimeCounter = slideTime;
        Speed = 2;
        animator = GetComponent<Animator>();
    }


    private void Update()
    {
        if (Velocity.x == 0 && grounded)
        {
            state = MovementState.G_Idle;
        }
        else if (!grounded)
        {
            state = MovementState.Airborne;
        }
        else
        {
            state = MovementState.G_Running;
            animator.SetFloat("Xvel", Mathf.Abs(xVel));
            
        }
        
        if(xVel < 0)
        {
            transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        if(xVel > 0)
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        
        


        xVel = Mathf.Lerp(xVel, Input.GetAxisRaw("Horizontal"), Time.deltaTime * 20);

        Velocity = new Vector2(xVel * Speed, Velocity.y);



        if (Input.GetButtonDown("Jump") && grounded)
        {
            isJumping = true;
            Velocity = new Vector2(Velocity.x, jumpForce);
        }

        
        if (Input.GetButton("Jump") && isJumping)
        {
            
            if(jumpTimeCounter > 0)
            {
                Velocity = new Vector2(Velocity.x, jumpForce);
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
 /*
        if(state == MovementState.G_Running)
        {
            
            if (Input.GetButtonDown("Crouch") && !isSliding)
            {
                xVel = (int)Input.GetAxisRaw("Horizontal");
                slideTimeCounter = slideTime;
                isSliding = true;
                transform.localScale = new Vector3(1, 0.5f);
            }

            if (Input.GetButton("Crouch") && isSliding)
            {
                if(slideTimeCounter > 0)
                {
                    slideTimeCounter -= Time.deltaTime;
                    Velocity = new Vector2(xVel * slideForce, Velocity.y);
                }
                else
                {
                    transform.localScale = new Vector3(1, 1f);
                    isSliding = false;
                    slideTimeCounter = 0;
                }
                  
                
            }
            else
            {
                transform.localScale = new Vector3(1, 1f);
                isSliding = false;
            }           
        }
        else
        {
            isSliding = false;
        }
        */

        
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
            Velocity = new Vector2(Velocity.x, 0);
        }
        wasGrounded = grounded;

        RigidBody.MovePosition(RigidBody.position + Velocity * Time.fixedDeltaTime);
    }

    bool Grounded()
    {
        return Physics2D.OverlapArea(new Vector2(transform.position.x - worldCollider.bounds.extents.x, transform.position.y - worldCollider.bounds.extents.y),
          new Vector2(transform.position.x + worldCollider.bounds.extents.x, transform.position.y - worldCollider.bounds.extents.y - 0.005f), groundLayer);
    }


    private void OnDrawGizmos()
    {
      
    }

    private void FixedUpdate()
    {
        Move();
    }
}
