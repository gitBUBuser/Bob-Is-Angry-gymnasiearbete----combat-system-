using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Entity , IMoving , IHealth
{
    [SerializeField]
    LayerMask groundLayer;
    [SerializeField]
    Transform feetTransform;

    [SerializeField]
    MovementState state;


    public float Speed { get; set; }
    public int Health { get; set; }
    public Vector2 Velocity { get; set; }
    public Vector2 OutsideForces { get; set; }
    public MovementState State { get { return state; } set { state = value; } }

    float jumpTimeCounter;
    float jumpTime = 0.2f;
    float jumpForce = 3f;
    float slideTime = 0.65f;
    float slideForce = 2.5f;
    float slideTimeCounter;
    float orgSpeed;
    float RA_speedMultiplier = 0.5f;

    float xVel;

    bool wasGrounded;
    bool grounded;
    bool isJumping;
    bool wallHugging;

    Animator animator;
    CapsuleCollider2D worldCollider;

    public enum MovementState
    {
        Airborne,
        G_Idle,
        G_Running,
        G_Sliding,
        Attacking
    }

    protected override void Start()
    {
        base.Start();
        worldCollider = GetComponent<CapsuleCollider2D>();
        xVel = 0;
        jumpTimeCounter = jumpTime;
        slideTimeCounter = slideTime;
        Speed = 2;
        Health = 1000;
        orgSpeed = Speed;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int amount)
    {
        Health -= amount;
    }

    private void Update()
    {
        if(state != MovementState.Attacking && state != MovementState.G_Sliding)
        {
            SetState();

            if (Input.GetButtonDown("LightAttack"))
            {
                switch (state)
                {
                    case MovementState.G_Running:
                        TellAnimatorAttackDirection();
                        state = MovementState.Attacking;
                        animator.SetBool("LightAttack", true);
                     
                        break;

                    case MovementState.Airborne:
                        TellAnimatorAttackDirection();
                        state = MovementState.Attacking;
                        animator.SetBool("LightAttack", true);
                        break;
                }
                Speed *= RA_speedMultiplier;

            }
        }

        if (Input.GetButtonDown("Crouch") && state == MovementState.G_Running)
        {
            xVel = Mathf.Lerp(xVel, Input.GetAxisRaw("Horizontal"), Time.deltaTime * 20);
            animator.SetBool("Slide", true);
            state = MovementState.G_Sliding;
        }

        if(state == MovementState.G_Sliding)
        {
            if (Input.GetButton("Crouch") && slideTimeCounter > 0)
            {
                slideTimeCounter -= Time.deltaTime;
                Speed = slideForce;

            }
            else if(!Patted())
            {
                ResetSlide();
            }
        }
        else
        {           
            xVel = Mathf.Lerp(xVel, Input.GetAxisRaw("Horizontal"), Time.deltaTime * 20);
        }

        animator.SetFloat("Xvel", Mathf.Abs(xVel));

        if(state != MovementState.Attacking)
        {
            if (xVel < 0)
            {
                transform.rotation = new Quaternion(0, 180, 0, 0);
            }
            if (xVel > 0)
            {
                transform.rotation = new Quaternion(0, 0, 0, 0);
            }
        }
        
        
        if(Mathf.Abs(xVel) < 0.01f)
        {
            xVel = 0;
        }

        
        Velocity = new Vector2(xVel * Speed, RigidBody.velocity.y);
        Jump();
    }

    public void Move()
    {
        grounded = Grounded();
        animator.SetBool("Grounded", grounded);
        RigidBody.velocity = new Vector2(Velocity.x, RigidBody.velocity.y) + OutsideForces;
        OutsideForces = Vector2.zero;
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && grounded)
        {
            if (!Patted())
            {
                isJumping = true;
                jumpTimeCounter = jumpTime;
                RigidBody.velocity = new Vector2(RigidBody.velocity.x, jumpForce);

                if (state == MovementState.G_Sliding)
                {
                    ResetSlide();
                }
            }
            
        }

        if (Input.GetButton("Jump") && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                RigidBody.velocity = new Vector2(RigidBody.velocity.x, jumpForce);
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
    }

    bool Grounded()
    {
        return Physics2D.OverlapArea(new Vector2(transform.position.x + worldCollider.offset.x - worldCollider.bounds.extents.x, transform.position.y + worldCollider.offset.y - worldCollider.bounds.extents.y),
          new Vector2(transform.position.x + worldCollider.bounds.extents.x + worldCollider.offset.x, transform.position.y - worldCollider.bounds.extents.y + worldCollider.offset.y - 0.001f), groundLayer);
    }

    bool Patted()
    {
        return Physics2D.OverlapArea(new Vector2(transform.position.x + worldCollider.offset.x - worldCollider.bounds.extents.x, transform.position.y + worldCollider.offset.y + worldCollider.bounds.extents.y),
          new Vector2(transform.position.x + worldCollider.bounds.extents.x + worldCollider.offset.x, transform.position.y + worldCollider.bounds.extents.y + worldCollider.offset.y + 0.2f), groundLayer);
    }

    void ResetSlide()
    {
        Speed = orgSpeed;
        slideTimeCounter = slideTime;
        animator.SetBool("Slide", false);
        SetState();
    }
    
    void SetState()
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
        }
    }

    void TellAnimatorAttackDirection()
    {
        if (Input.GetButton("Vertical"))
        {
            if(Input.GetAxisRaw("Vertical") < 0)
            {
                animator.SetInteger("Direction", 2);
            }
            else
            {
                animator.SetInteger("Direction", 3);
            }
        }
        else if (Input.GetButton("Horizontal"))
        {
            animator.SetInteger("Direction", 1);
        }
        else
        {
            animator.SetInteger("Direction", 3);
        }
    }

    private void FixedUpdate()
    {
        Move();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Chunk")
        {
            collision.transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(Velocity.x * 0.00003f, 0.00003f), ForceMode2D.Impulse);
        }
    }
    ///ANIMATION CONTROLS
    ///

    public void LightAttackIsDone()
    {
        animator.SetBool("LightAttack", false);
        Speed = orgSpeed;
        SetState();
    }
}
