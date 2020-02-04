using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieJock : GroundedEnemy
{
    [SerializeField]
    float maxJumpDistance;
    [SerializeField]
    float minJumpDistance;
    [SerializeField]
    float speed;
    Animator myAnim;
    [SerializeField]
    Transform attackT;
    [SerializeField]
    GameObject attackSmoke;
    [SerializeField]
    Transform smokeSpawn;
    [SerializeField]
    Transform runSmokeSpawn;
    [SerializeField]
    GameObject runSmoke;

    bool coolingDown;
    bool attacked = false;

    enum States
    {
        Idle,
        Walking,
        Attacking
    }

    States state = States.Idle;

    protected override void Start()
    {
        base.Start();
        myAnim = GetComponent<Animator>();
    }

    public override void Stun()
    {
        myAnim.SetBool("Stunned", true);
        base.Stun();
    }

    protected override void StunIsFinished()
    {
        myAnim.SetBool("Stunned", false);
        base.StunIsFinished();
    }

    private void Update()
    {
        myAnim.SetBool("Grounded", OnGround);
        myAnim.SetFloat("Velocity", Mathf.Abs(RB.velocity.x));

        if (Stunned)
        {
            if (OnGround)
            {
                if (Timer(ref stunTimer, StunTime))
                {
                    StunIsFinished();
                }
                else return;
            }
            else return;
        }

        if (coolingDown)
        {           
            return;
        }

        switch (state)
        {
            case States.Idle:
                if (CanSeePlayer())
                {
                    state = States.Attacking;
                }
                break;

            case States.Walking:
                if (CanSeePlayer())
                {
                    state = States.Attacking;
                }
                break;

            case States.Attacking:
                float distance = Vector2.Distance(transform.position, Player.transform.position);
                if (CanReachPlayer(distance))
                {
                    if (OnGround)
                    {
                        JumpTowardsPlayer();
                    }
                }
                else
                {
                    if(distance > maxJumpDistance)
                    {
                        ChasePlayer();
                    }

                    if(distance < minJumpDistance)
                    {
                        BackAway();
                    }

                    InputMove(transform.right * speed);
                }
                break;
        }

    }

    protected override void OnGroundedEnter()
    {      
        if (attacked)
        {
            attacked = false;
            float distance = Vector2.Distance(transform.position, Player.transform.position);
            if(distance < 10)
            {
                ShakeTransformEventData seData = new ShakeTransformEventData();
                seData.duration = 1f;
                seData.amplitude = 10 - distance;
                seData.frequency = 3;
                Player.GetComponent<PlayerController>().CamShake(seData);
                
            }           
            myAnim.SetTrigger("Landed");
            GameObject test = Instantiate(attackSmoke, smokeSpawn.position, transform.rotation);
            RegisterAttack();
            StartCoroutine(RecoverFromAttack());
        }
        base.OnGroundedEnter();
    }

    protected override void OnGroundedExit()
    {
        stunTimer = 0;
        base.OnGroundedExit();
    }

    bool CanReachPlayer(float distance)
    {
        // and player is grounded
        return distance < maxJumpDistance && distance > minJumpDistance;
    }

    void JumpTowardsPlayer()
    {
        ChasePlayer();
        if (Player.GetComponent<PlayerController>().OnGround)
        {
            StartCoroutine(JumpAttack());
        }
    }

    IEnumerator RecoverFromAttack()
    {
        yield return new WaitForSeconds(2.5f);
        myAnim.SetTrigger("DoneRecovering");
        yield return new WaitForSeconds(1f);
        coolingDown = false;
        ResetMaxSpeed();
    }

    IEnumerator JumpAttack()
    {
        coolingDown = true;
        attacked = true;
        myAnim.SetTrigger("StartJump");
        yield return new WaitForSeconds(0.1f);
        MaxedSpeed();
        Vector2 distance = Player.transform.position - transform.position;
        Vector2 jumpForce = distance;
        jumpForce.y += 2.5f;
        RB.velocity = jumpForce * 2;
        yield return new WaitForSeconds(0.1f);
        myAnim.SetTrigger("Jump");
    }

    public void SpawnRunSmoke()
    {
        Instantiate(runSmoke, runSmokeSpawn.position, transform.rotation);
    }

    void ChasePlayer()
    {
        if (Player.transform.position.x - transform.position.x < 0)
        {
            transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        else
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }
    }

    void RegisterAttack()
    {
        Collider2D[] collider = Physics2D.OverlapBoxAll(attackT.position,
            new Vector2(4f, 1.2f), playerLayer);

        for (int i = 0; i < collider.Length; i++)
        {
            if (collider[i].gameObject.GetComponent<PlayerController>())
            {
                PlayerController controller = collider[i].gameObject.GetComponent<PlayerController>();
                controller.TakeDamage(6);
                float xDistance = Player.transform.position.x - transform.position.x;
                Vector2 knockback = new Vector2(xDistance, 0.5f).normalized;
                controller.Knockback(knockback * 7);
            }
        }
    }

    void BackAway()
    {
        if (Player.transform.position.x - transform.position.x > 0)
        {
            transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        else
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, maxJumpDistance);
        Gizmos.DrawWireSphere(transform.position, minJumpDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackT.position, new Vector3(4, 1.2f, 0));
    }


}
