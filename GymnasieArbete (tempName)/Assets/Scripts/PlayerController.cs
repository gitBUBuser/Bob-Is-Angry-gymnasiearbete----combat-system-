using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : Entity
{
    [SerializeField]
    Transform feetTransform;

    [SerializeField]
    Transform cameraParent;

    [SerializeField]
    MovementState state;

    [SerializeField]
    float stunTime;

    PlayerCombat combatController;
    PlayerFX fx;
    public float Speed { get; set; }

    public MovementState State { get { return state; } set { state = value; } }
    float jumpTimeCounter;
    float jumpTime = 0.2f;
    float jumpForce = 6f;
    float slideTime = 0.65f;
    float slideForce = 12f;
    float slideTimeCounter;
    float orgSpeed;
    float RA_speedMultiplier = 0.5f;
    float xVel;

    bool isJumping;
    bool secondJump;
    bool slideAttackIsReady;
    Animator animator;
    ShakeTransform cameraShake;


    public enum MovementState
    {
        Airborne,
        G_Idle,
        G_Running,
        G_Sliding,
        Attacking
    }
    private void Awake()
    {
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    protected override void Start()
    {
        base.Start();       
        jumpTimeCounter = jumpTime;
        slideTimeCounter = slideTime;
        Speed = 30;
        orgSpeed = Speed;
        animator = GetComponent<Animator>();
        combatController = GetComponent<PlayerCombat>();
        fx = GetComponent<PlayerFX>();
    }

    public void CamShake(ShakeTransformEventData data)
    {
        fx.CamShake(data);
    }

    public void ComboPlus()
    {
        Camera.main.transform.Find("Canvas").Find("ComboCounter").GetComponent<ComboCounter>().Combo();
    }

    public void ResetCombo()
    {
        Camera.main.transform.Find("Canvas").Find("ComboCounter").GetComponent<ComboCounter>().Reset();
    }

    public override void GetHit(int damage, Vector2 knockback)
    {
        if (!Stunned)
        {
            base.GetHit(damage, knockback);
            StartCoroutine(StartStun());
        }
        
    }

    void Update()
    {
        if (Stunned)
        {
            return;
        }
            

        if(state != MovementState.G_Sliding && state != MovementState.Attacking)
        {
            SetState();
        }
        SetXvel(Input.GetAxisRaw("Horizontal"));
        animator.SetFloat("Xvel", Mathf.Abs(xVel));
        Slide();
       

        NewJump();
        if (Input.GetButtonDown("LightAttack"))
        {
           // cameraParent.GetComponent<Animator>().SetTrigger("Attack");
            LightAttack();
        }

        if (state != MovementState.Attacking)
        {
            if (xVel < 0)
                transform.rotation = new Quaternion(0, 180, 0, 0);
            if (xVel > 0)
                transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        InputMove(new Vector2(xVel * Speed, 0));
    }

    public void LightAttack()
    {
        if (state != MovementState.Attacking && state != MovementState.G_Sliding)
        {
            SetState();
            switch (state)
            {
                case MovementState.G_Idle:
                    state = MovementState.Attacking;
                    MaxSpeed = 4f;
                    animator.SetBool("LightAttack", true);
                    break;

                case MovementState.G_Running:
                    TellAnimatorAttackDirection();
                    state = MovementState.Attacking;
                    MaxSpeed = 4f;
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

    void SetXvel(float newXVel)
    {
        if (state != MovementState.G_Sliding)
        {
            xVel = newXVel;
        }
    }

    void Slide()
    {
        if (Input.GetButtonDown("Crouch"))
        {
            if (state == MovementState.G_Running && OnGround)
            {

                slideTimeCounter = slideTime;
                animator.SetBool("Slide", true);
                state = MovementState.G_Sliding;
                MaxSpeed = slideForce;
            }                      
        }
        if (Input.GetButton("Crouch"))
        {
            if (slideAttackIsReady)
            {
                combatController.Slide();
            }

            if (slideTimeCounter > 0)
            {
                slideTimeCounter -= Time.deltaTime;
            }
            else
            {
                StopSlide();
            }
        }
        else
        {
            StopSlide();
        }
    }

    public void NewJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (OnGround)
            {
                if (!Patted())
                {
                    isJumping = true;
                    Jump(jumpForce);
                    jumpTimeCounter = jumpTime;

                    if (state == MovementState.G_Sliding)
                    {
                        StopSlide();
                    }
                    fx.JumpingParticle();
                    animator.SetBool("Jump", true);
                }
            }
            else if (secondJump)
            {
                secondJump = false;
                if (!Patted())
                {
                    isJumping = true;
                    jumpTimeCounter = jumpTime;
                    Jump(jumpForce);
                    fx.JumpingParticle();
                }
            }
        }
        HoldJump();
    }

    public void HoldJump()
    {
        if (isJumping)
        {
            if (jumpTimeCounter > 0 && Input.GetButton("Jump"))
            {
                Jump(jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
    }

    void StopSlide()
    {
        if (!Patted() && state == MovementState.G_Sliding)
        {
            combatController.ResetList();
            ResetMaxSpeed();
            Speed = orgSpeed;
            slideTimeCounter = slideTime;
            animator.SetBool("Slide", false);
            SetState();
        }
    }

    void SetState()
    {
        if (RB.velocity.x == 0 && OnGround)
            state = MovementState.G_Idle;
        else if (!OnGround)
            state = MovementState.Airborne;
        else
            state = MovementState.G_Running;
    }

    IEnumerator StartStun()
    {
        yield return new WaitForSeconds(stunTime);
        Stunned = false;
        ResetMaxSpeed();
    }

    void AnimSlideAttack()
    {
        slideAttackIsReady = true;
    }

    void AnimSlideStop()
    {
        slideAttackIsReady = false;
    }

    void TellAnimatorAttackDirection()
    {
        Vector2 lookDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (lookDir.y != 0)
        {
            if(lookDir.y < 0)
            {
                animator.SetInteger("Direction", 2);
            }         
        }
        else if (lookDir.x != 0)
        {
            animator.SetInteger("Direction", 1);
        }
        else
        {
            animator.SetInteger("Direction", 3);
        }
        
    }

    int InputToRaw(float input)
    {
        if(input > 0)
        {
            input = 1;
        }
        if (input < 0)
        {
            input = -1;
        }
        return (int)input;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        animator.SetBool("Grounded", OnGround);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    protected override void OnGroundedEnter()
    {
        base.OnGroundedEnter();
        secondJump = true;
        fx.LandingParticle();
    }


    ///ANIMATION CONTROLS
    ///

    public void LightAttackIsDone()
    {
        animator.SetBool("LightAttack", false);
        Speed = orgSpeed;
        SetState();
        ResetMaxSpeed();
    }

    public void JumpIsOver()
    {
        animator.SetBool("Jump", false);
    }
}
