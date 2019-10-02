using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    Rigidbody2D rigidBody;

    [SerializeField]
    LayerMask groundLayer;

    [SerializeField]
    CapsuleCollider2D worldCollider;

    [SerializeField]
    float mass;

    [SerializeField]
    float maxSpeed;
    float orgMaxSpeed;
    float groundedTime;

    bool previousGrounded;
    bool previousPatted;
    public bool OnGround { get; set; }
    public bool TouchingCeiling { get; set; }
    public float MaxSpeed { get { return maxSpeed; } set { maxSpeed = value; } }

    public Rigidbody2D RB { get { return rigidBody; } }
    public const float Gravity = 1f;

    Vector2 moveForce;
    Vector2 gravityForce;
    Vector2 sumOfForces;
    Vector2 velocity;
    Vector2 acceleration;
    Vector2 friction;

    public Vector2 AccessVelocity { get { return velocity; } set { velocity = value; } }

    protected virtual void Start()
    {
        orgMaxSpeed = maxSpeed;
        gravityForce = new Vector2(0, -Gravity);
        rigidBody = GetComponent<Rigidbody2D>();
        worldCollider = GetComponent<CapsuleCollider2D>();
    }

    public void AddForce(Vector2 force)
    {
        sumOfForces += force;   
    }
    
    protected void ResetSpeed()
    {
        maxSpeed = orgMaxSpeed;
    }

    public void Jump(float jumpForce)
    {
        velocity.Set(velocity.x, jumpForce);
    }

    protected void InputForce(Vector2 force)
    {       
        moveForce = force;
    }

    bool Grounded()
    {
        return Physics2D.OverlapArea(new Vector2(transform.position.x + worldCollider.offset.x - worldCollider.bounds.extents.x + 0.1f, transform.position.y + worldCollider.offset.y - worldCollider.bounds.extents.y),
          new Vector2(transform.position.x + worldCollider.bounds.extents.x + worldCollider.offset.x - 0.1f, transform.position.y - worldCollider.bounds.extents.y + worldCollider.offset.y - 0.04f), groundLayer);
    }

    public bool Patted()
    {
        return Physics2D.OverlapArea(new Vector2(transform.position.x + worldCollider.offset.x - worldCollider.bounds.extents.x + 0.1f, transform.position.y + worldCollider.offset.y + worldCollider.bounds.extents.y),
          new Vector2(transform.position.x + worldCollider.bounds.extents.x + worldCollider.offset.x - 0.1f, transform.position.y + worldCollider.bounds.extents.y + worldCollider.offset.y + 0.2f), groundLayer);
    }

    protected void Move()
    {
        if (OnGround)                 
            gravityForce.Set(0, 0);
        else
            gravityForce.Set(0, -Gravity * mass);

        if (Mathf.Abs(velocity.x) > 0.4f * mass)
            friction = new Vector2(-velocity.normalized.x * Gravity * mass * 0.5f, 0);
        else
            velocity.x = 0;
       
        AddForce(gravityForce + moveForce + friction);
        acceleration = sumOfForces / mass;
        velocity += acceleration;

        if (Mathf.Abs(velocity.x) > maxSpeed)
            velocity.x = maxSpeed * velocity.normalized.x;
        
        RB.MovePosition((Vector2)transform.position + velocity * Time.fixedDeltaTime);
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

        if (!previousPatted && TouchingCeiling)
            OnPattedEnter();
    }

    /// <summary>
    /// GOES AFTER COLLISIONCHECK() AND MOVE()
    /// </summary>
    protected void ResetPhysicsValues()
    {
        sumOfForces.Set(0, 0);
        moveForce.Set(0, 0);
        friction.Set(0, 0);

        previousGrounded = OnGround;
        previousPatted = TouchingCeiling;
    }

    protected virtual void OnPattedEnter()
    {
        if(velocity.y > 0)
            velocity.y = 0;
    }

    protected virtual void OnGroundedEnter()
    {
        velocity.y = 0;
    }


    protected virtual void FixedUpdate()
    {
        CollisionChecks();
        Move();
        ResetPhysicsValues();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Entity>())
        {

           // AddForce(new Vector2(-1000 * transform.right.x * Mathf.Abs(collision.gameObject.GetComponent<Entity>().velocity.x + 1), 1));


        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Entity>())
        {

          //  AddForce(new Vector2(-1000 * transform.right.x * Mathf.Abs(collision.gameObject.GetComponent<Entity>().velocity.x + 1), 1));


        }
    }
}
