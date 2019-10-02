using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Entity, IHealth
{
    [SerializeField]
    Transform feetTransform;

    [SerializeField]
    MovementState state;


    public float Speed { get; set; }
    public int Health { get; set; }
    public MovementState State { get { return state; } set { state = value; } }
    bool previouslyPatted;
    float jumpTimeCounter;
    float jumpTime = 0.25f;
    float jumpForce = 10;
    float slideTime = 0.65f;
    float slideForce = 12f;
    float slideTimeCounter;
    float orgSpeed;
    float RA_speedMultiplier = 0.5f;
    float groundTime = 0;
    float xVel;

    bool wasGrounded;
    public bool grounded;
    bool isJumping;
    bool wallHugging;

    Animator animator;
    Vector2 inputForce;

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
        xVel = 0;
        jumpTimeCounter = jumpTime;
        slideTimeCounter = slideTime;
        Speed = 5;
        Health = 1000;
        orgSpeed = Speed;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int amount)
    {
        Health -= amount;
    }

    void Update()
    {
        if (state != MovementState.Attacking && state != MovementState.G_Sliding)
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
            xVel = Input.GetAxisRaw("Horizontal");
            animator.SetBool("Slide", true);
            state = MovementState.G_Sliding;
        }

        if (state == MovementState.G_Sliding)
        {
            if (Input.GetButton("Crouch") && slideTimeCounter > 0)
            {
                slideTimeCounter -= Time.deltaTime;
                Speed = slideForce;
                MaxSpeed = slideForce;

            }
            else if (!Patted())
            {
                ResetSlide();
            }
        }
        else
        {
            xVel = Input.GetAxisRaw("Horizontal");
        }

        animator.SetFloat("Xvel", Mathf.Abs(xVel));

        if (state != MovementState.Attacking)
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
       
        Jump();
        InputForce(new Vector2(xVel * Speed, 0));
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && OnGround)
        {
            animator.SetBool("Jump", true);

            if (!Patted())
            {
                isJumping = true;
                jumpTimeCounter = jumpTime;
                Jump(jumpForce);

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

                inputForce.Set(inputForce.x, jumpForce);
                Jump(jumpForce);
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

    void ResetSlide()
    {
        MaxSpeed = 8;
        Speed = orgSpeed;
        slideTimeCounter = slideTime;
        animator.SetBool("Slide", false);
        SetState();
    }

    void SetState()
    {
        if (AccessVelocity.x == 0 && OnGround)
            state = MovementState.G_Idle;
        else if (!OnGround)
            state = MovementState.Airborne;
        else
            state = MovementState.G_Running;
    }

    protected override void OnPattedEnter()
    {
        if (AccessVelocity.y > 0)
        {
            AccessVelocity.Set(AccessVelocity.x, 0);
            isJumping = false;
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

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        animator.SetBool("Grounded", OnGround);
    }
   

    ///ANIMATION CONTROLS
    ///


   

    public void LightAttackIsDone()
    {
        animator.SetBool("LightAttack", false);
        Speed = orgSpeed;
        SetState();
    }

    public void JumpIsOver()
    {
        animator.SetBool("Jump", false);
    }
}
