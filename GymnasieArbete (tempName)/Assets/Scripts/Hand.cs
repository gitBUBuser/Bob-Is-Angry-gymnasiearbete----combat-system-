using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : GroundedEnemy
{   
    public int Health { get; set; }

    Animator animator;

    [SerializeField] Transform attackPosition;
    [SerializeField] float attackRadius;
    [SerializeField] float speed;
    float orgAngle;
    [SerializeField] float groundedTimer;
    [SerializeField] float groundTime;

    bool attackHasStarted = false;

    enum States
    {
        Idle,
        Walking,
        Chasing,
        Attacking
    }

    [SerializeField] States state = States.Idle;

    protected override void Start()
    {
        base.Start();
        orgAngle = 40;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!OnGround)
        {
            groundedTimer = 0;
        }
        else
        {
            groundedTimer += Time.deltaTime;
        }

        if(Stunned)
        {
            if (Timer(ref stunTimer, StunTime))
            {
                StunIsFinished();
                DecideState();
            }             
        }

        else
        {
            switch (state)
            {
                case States.Idle:
                    SetViewAngle(orgAngle);
                    animator.SetInteger("AttackState", 0);
                    if (Timer(ref idleTimer, IdleTime))
                    {
                        DecideState();
                    } 
                    break;

                case States.Walking:
                    SetViewAngle(orgAngle);
                    animator.SetInteger("AttackState", 0);
                    if(JumpIsNeeded() && groundedTimer > groundTime)
                    {
                        RB.velocity = new Vector2(RB.velocity.x, JumpForce);
                    }
                    InputMove(new Vector2(PatrolPoints[CurrentPoint].position.x - transform.position.x, 0).normalized * speed);
                    SetRotation();
                    DecidePatrolPoint();
                    break;

                case States.Chasing:
                    SetViewAngle(180);
                    animator.SetInteger("AttackState", 0);
                    if (JumpIsNeeded() && groundedTimer > groundTime)
                    {
                        RB.velocity = new Vector2(RB.velocity.x, JumpForce);
                    }
                    InputMove(new Vector2(Player.transform.position.x - transform.position.x, 0).normalized * speed);
                    SetRotation();
                    break;

                case States.Attacking:
                    if(animator.GetInteger("AttackState") < 1)
                    {
                        animator.SetInteger("AttackState", 1);
                    }                  
                    break;
            }
        }      
    }

    protected override void FixedUpdate()
    {
        if(state != States.Idle && state != States.Attacking)
        {
            DecideState();
        }
        base.FixedUpdate();
    }

    protected override void OnPointSwitch()
    {
        idleTimer = 0;
        state = States.Idle;
    }

    void DecideState()
    {      
        States wantedState = state;

        if (CanSeePlayer())
        {
            wantedState = States.Chasing;
        }
        else
        {
            wantedState = States.Walking;
        }

        if (wantedState == States.Chasing)
        {
            if (Physics2D.OverlapCircle(attackPosition.position, attackRadius, playerLayer))
            {
                wantedState = States.Attacking;                
            }
        }
        state = wantedState;
    }

    void SetRotation()
    {
        switch (state)
        {
            case States.Chasing:
                if (Mathf.Abs(RB.velocity.x) > 0)
                {
                    if (RB.velocity.x < 0)
                    {
                        transform.rotation = new Quaternion(0, 180, 0, 0);
                    }
                    else
                        transform.rotation = new Quaternion(0, 0, 0, 0);
                }
                break;

            case States.Walking:
                if(PatrolPoints[CurrentPoint].position.x - transform.position.x < 0)
                {
                    transform.rotation = new Quaternion(0, 180, 0, 0);
                }
                else
                {
                    transform.rotation = new Quaternion(0, 0, 0, 0);
                }
                break;
        }
       
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireSphere(attackPosition.position, attackRadius);
    }
    
    public void PlayerStillInRange()
    {
        if (Physics2D.OverlapCircle(attackPosition.position, attackRadius, playerLayer))
        {
            animator.SetInteger("AttackState", 2);
        }
        else
        {
            DecideState();
        }
    }
    
    public void Attack()
    {
        PlayerController pC = Player.GetComponent<PlayerController>();
        float xDistance = Player.transform.position.x - transform.position.x;
        pC.RB.velocity = new Vector2(xDistance / Mathf.Abs(xDistance) * 2, 1) * 6;
    }

    public void ResetAttack()
    {
        DecideState();
    }
}
