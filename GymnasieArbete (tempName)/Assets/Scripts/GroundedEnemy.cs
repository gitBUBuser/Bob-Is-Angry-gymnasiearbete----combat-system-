using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedEnemy : Entity
{

    public int CurrentPoint { get; set; } = 0;

    bool wantsToJump;
    protected float idleTimer;
    protected float stunTimer;

    protected float StunTime { get { return stunTime; } }
    protected float IdleTime { get { return idleTime; } }
    protected float JumpForce { get { return jumpForce; } }
    protected GameObject Player { get; set; }

    protected Transform[] PatrolPoints { get { return patrolPoints; } }
    

    [SerializeField] float idleTime;
    [SerializeField] float stunTime;
    [SerializeField] float viewRadius;
    [SerializeField] float innerRadius;
    [SerializeField] float viewAngle;
    [SerializeField] float distanceToJump;
    [SerializeField] Transform feetTransform;
    [SerializeField] float jumpForce;
    [SerializeField] float jumpRaycastOffset;

    [SerializeField] public LayerMask playerLayer;
    [SerializeField] LayerMask inWayOfView;
    [SerializeField] LayerMask Obstacles;
    [SerializeField] LayerMask GroundLayer;
    [SerializeField] Transform[] patrolPoints;

    

    protected override void Start()
    {
        base.Start();
        idleTimer = idleTime;
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    public bool Timer(ref float timer, float maxTime)
    {
        timer += Time.deltaTime;
        if(timer > maxTime)
        {
            return true;
        }
        return false;
    }

    protected bool WalkingCloseToLedge()
    {
        if (wantsToJump)
        {
            return false;
        }
        return !Physics2D.Raycast(feetTransform.position + (transform.right * jumpRaycastOffset), Vector2.down, 1, GroundLayer);
    }

    public bool JumpIsNeeded()
    {
        if(Physics2D.Raycast(feetTransform.position, transform.right, jumpRaycastOffset, Obstacles))
        {
            wantsToJump = true;
        }
        else
        {
            wantsToJump = false;
        }
        return Physics2D.Raycast(feetTransform.position, transform.right, 0.2f, Obstacles);
    }

    public void DecidePatrolPoint()
    {
        if (WalkingCloseToLedge() && OnGround)
        {
            OnPointSwitch();
            if (CurrentPoint != patrolPoints.Length - 1)
                CurrentPoint++;
            else
                CurrentPoint = 0;
        }
        if (Vector2.Distance(transform.position, patrolPoints[CurrentPoint].position) < 0.5f)
        {
            OnPointSwitch();
            if (CurrentPoint != patrolPoints.Length - 1)
                CurrentPoint++;
            else
                CurrentPoint = 0;
            
        }
    }

    protected override void Stun()
    {
        Stunned = true;
        MaxedSpeed();
        stunTimer = 0;
    }

    protected virtual void StunIsFinished()
    {
        Stunned = false;
        ResetMaxSpeed();       
    }

    protected void SetViewAngle(float angle)
    {
        viewAngle = angle;
    }

    protected bool CanSeePlayer()
    {
        if (Physics2D.OverlapCircle(transform.position, viewRadius, playerLayer))
        {
            Vector2 difference = Player.transform.position - transform.position;
            float sign = (Player.transform.position.y > transform.position.y) ? -1.0f : 1.0f;
            float angle = Vector2.Angle(transform.right, difference) * sign;
            if (angle < viewAngle && angle > -viewAngle)
            {
              
                return (Physics2D.Raycast(transform.position, Player.transform.position - transform.position, 100, ~inWayOfView).transform.tag == "Player");
            }
        }
        return false;
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, viewRadius);
        Gizmos.DrawRay(feetTransform.position, transform.right);
        Gizmos.DrawRay(feetTransform.position + transform.right * jumpRaycastOffset, Vector2.down);
    }

    protected virtual void OnPointSwitch() { }
}
