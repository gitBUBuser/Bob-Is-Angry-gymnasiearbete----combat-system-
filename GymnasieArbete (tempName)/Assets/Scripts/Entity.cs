using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public bool Stunned { get; set; }
    Rigidbody2D rigidBody;

    [SerializeField]
    LayerMask groundLayer;
    [SerializeField]
    CapsuleCollider2D worldCollider;

    [SerializeField]
    float maxSpeed;
    float orgMaxSpeed;
    float groundedTime;

    [SerializeField]
    public int maxHealth;
    public int currentHealth;

    bool previousGrounded;
    bool previousPatted;
    public bool OnGround { get; set; }
    public bool TouchingCeiling { get; set; }
    public float MaxSpeed { get { return maxSpeed; } set { maxSpeed = value; } }

    public Rigidbody2D RB { get { return rigidBody; } }

    Vector2 moveForce;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        orgMaxSpeed = maxSpeed;
        rigidBody = GetComponent<Rigidbody2D>();
        worldCollider = GetComponent<CapsuleCollider2D>();
    }

    public virtual void GetHit(int damage, Vector2 knockback)
    {
        Stun();
        TakeDamage(damage);
        RB.velocity = knockback;
    }

    protected virtual void Stun()
    {
        Stunned = true;
    }

    protected virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }

    protected void ResetMaxSpeed()
    {
        maxSpeed = orgMaxSpeed;
    }

    public void InputMove(Vector2 velocity)
    {
        moveForce = velocity;
    }

    public void Jump(float jumpForce)
    {
        RB.velocity = new Vector2(RB.velocity.x, jumpForce);
    }

    bool Grounded()
    {
        Vector2 offset = worldCollider.offset - new Vector2(0, worldCollider.size.y / 2);
        return Physics2D.OverlapCircle((Vector2)transform.position + offset, 0.2f, groundLayer);
    }

    public bool Patted()
    {
        Vector2 offset = worldCollider.offset + new Vector2(0, worldCollider.size.y / 2);
        return Physics2D.OverlapCircle((Vector2)transform.position + offset, 0.2f, groundLayer);
    }

    protected void Move()
    {
        Vector2 offset = worldCollider.offset;
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + offset, moveForce.normalized, worldCollider.size.x + 0.1f, groundLayer);

        if (!hit)
        {
            RB.AddForce(moveForce);
        }
        
        if (Mathf.Abs(RB.velocity.x) > maxSpeed)
        {
            RB.velocity = new Vector2(RB.velocity.normalized.x * MaxSpeed, RB.velocity.y);
        }
        
    }

    public void MaxedSpeed()
    {
        MaxSpeed = 1000;
    }

    /// <summary>
    /// GOES BEFORE MOVE()
    /// </summary>
    protected void CollisionChecks()
    {
        OnGround = Grounded();
        TouchingCeiling = Patted();

        if (!previousGrounded && OnGround)
            OnGroundedEnter();

        if (previousGrounded && !OnGround)
            OnGroundedExit();

        if (!previousPatted && TouchingCeiling)
            OnPattedEnter();
    }

    /// <summary>
    /// GOES AFTER COLLISIONCHECK() AND MOVE()
    /// </summary>
    protected void ResetPhysicsValues()
    {
        moveForce.Set(0, 0);
        previousGrounded = OnGround;
        previousPatted = TouchingCeiling;
    }

    protected virtual void OnPattedEnter()
    {

    }

    protected virtual void OnGroundedEnter()
    {
      
    }

    protected virtual void OnGroundedExit()
    {

    }

    protected virtual void FixedUpdate()
    {     
        CollisionChecks();
        Move();
        ResetPhysicsValues();
    }
}
