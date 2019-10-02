using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : Entity, IHealth
{
    [SerializeField]
    float idleTime,
        chaseTime;
    [SerializeField]
    LayerMask playerLayer;
    [SerializeField]
    float viewRadius;

    [SerializeField]
    Transform[] movePoints;
    GameObject player;

    int currentPoint;
    Animator animator;
    float speed = 2f;
    float jumpForce = 6;
    public float Speed { get { return speed; } set { speed = value; } }
    float groundTime = 0;
    bool grounded;
    public Vector2 Velocity { get; set; }
    public int Health { get; set; }
    bool knowsPlayerLocation;
    [SerializeField]
    State state;
    enum State
    {
        Moving,
        Chasing,
        Attacking
    }

    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player");
        state = State.Moving;
        animator = GetComponent<Animator>();
    }

  
    protected override void FixedUpdate()
    {
        CollisionChecks();
        DecideDirection();

        switch (state)
        {
            case State.Moving:
                if (idleTime > 2)
                    InputForce(transform.right * speed);
                break;

            case State.Chasing:
                InputForce(transform.right * speed);
                break;

            case State.Attacking:
                InputForce(transform.right * 0);
                break;
        }   
        Move();
        ResetPhysicsValues();

        knowsPlayerLocation = LookAround();

        DecideAction();       
    }

    void DecideDirection()
    {
        switch (state)
        {
            case State.Moving:

                if (movePoints[currentPoint].transform.position.x < transform.position.x)
                {
                    transform.rotation = new Quaternion(0, 180, 0, 0);
                }
                else
                {
                    transform.rotation = new Quaternion(0, 0, 0, 0);
                }
                if (Vector2.Distance(transform.position, movePoints[currentPoint].position) < 0.5f)
                {
                    idleTime = 0;
                    if (currentPoint != movePoints.Length - 1)
                    {
                        currentPoint++;
                    }
                    else
                    {
                        currentPoint = 0;
                    }

                }
                break;

            case State.Chasing:
                //if (knowsPlayerLocation)
              //  {
                    if (player.transform.position.x < transform.position.x)
                    {
                        transform.rotation = new Quaternion(0, 180, 0, 0);
                    }
                    else
                    {
                        transform.rotation = new Quaternion(0, 0, 0, 0);
                    }
               // }             
                break;
        }       
    }

    bool LookAround()
    {
        switch (state)
        {
            case State.Moving:
                if (Physics2D.OverlapCircle(transform.position, viewRadius, playerLayer))
                {
                    Vector2 difference = player.transform.position - transform.position;
                    float sign = (player.transform.position.y > transform.position.y) ? -1.0f : 1.0f;
                    float angle = Vector2.Angle(transform.right, difference) * sign;

                    if (angle < 45 && angle > -45)
                    {
                        if (Physics2D.Raycast(transform.position, player.transform.position - transform.position, 100, ~(1 << LayerMask.NameToLayer("Enemy"))).transform.tag == "Player")
                        {
                            if (state != State.Attacking)
                            {
                                state = State.Chasing;
                            }

                            return true;
                        }
                    }
                }
                break;
                
                

            case State.Chasing:
                if (Physics2D.OverlapCircle(transform.position, viewRadius, playerLayer))
                {
                    return true;                  
                }
                break;
        }
        
        return false;
    }

    void DecideAction()
    {
        switch (state)
        {
            case State.Chasing:
                if (Physics2D.OverlapCircle(new Vector2(transform.position.x + 0.34f * transform.right.x, transform.position.y), 0.65f, playerLayer)
                    || Vector2.Distance(transform.position, (Vector2)player.transform.position + player.GetComponent<PlayerController>().AccessVelocity * Time.fixedDeltaTime * 10) < 0.8f)
                {
                    animator.SetInteger("AttackState", 1);
                    state = State.Attacking;

                }
                
                break;

            case State.Attacking:
                if (Vector2.Distance(transform.position, (Vector2)player.transform.position + (player.GetComponent<PlayerController>().AccessVelocity * Time.fixedDeltaTime * 10)) < 0.8f)
                {
                    animator.SetInteger("AttackState", 1);
                }
                if (Physics2D.OverlapCircle(new Vector2(transform.position.x + 0.34f * transform.right.x, transform.position.y), 0.65f, playerLayer))
                {
                    animator.SetInteger("AttackState", 2);
                }
                if (Vector2.Distance(transform.position, (Vector2)player.transform.position + (player.GetComponent<PlayerController>().AccessVelocity * Time.fixedDeltaTime * 10)) > 3)
                {
                    AttackIsFinished();
                }
                if (player.transform.position.x > transform.position.x && transform.rotation == new Quaternion(0, 180, 0, 0))
                {
                    AttackIsFinished();
                }
                if (player.transform.position.x < transform.position.x && transform.rotation == new Quaternion(0, 0, 0, 0))
                {
                    AttackIsFinished();
                }
                break;
        }
    }

    public void TakeDamage(int amount)
    {
        Health -= amount;
    }

    private void Update()
    {
        idleTime += Time.deltaTime;
      
        if (!knowsPlayerLocation && state == State.Chasing)
        {
            chaseTime += Time.deltaTime;
            if (chaseTime > 1.5f)
            {
                chaseTime = 0;
                state = State.Moving;
            }
        }
        DecideAction();
        Velocity = new Vector2(transform.right.x * speed, Velocity.y - 25 * Time.deltaTime);
        
        if (grounded)
        {
            groundTime += Time.deltaTime;
            if (groundTime > 0.1f)
            {
                Velocity = new Vector2(Velocity.x, 0);
            }
        }
        else
        {
            groundTime = 0;
        }
    }

    void DirectionTowardsMe()
    {

    }

    ////ANIMATOR

    public void Attack()
    {       
        if(Physics2D.OverlapCircle(new Vector2(transform.position.x + 0.34f * transform.right.x, transform.position.y), 0.65f, playerLayer))
        {
            float xDistance = transform.position.x - player.transform.position.x;
            player.GetComponent<PlayerController>().AddForce(new Vector2(-xDistance * 10, 3));
        }
    }

    public void AttackIsFinished()
    {
        animator.SetInteger("AttackState", 0);
        state = State.Chasing;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(new Vector2(transform.position.x + 0.34f * transform.right.x, transform.position.y), 0.65f);
    }

}
