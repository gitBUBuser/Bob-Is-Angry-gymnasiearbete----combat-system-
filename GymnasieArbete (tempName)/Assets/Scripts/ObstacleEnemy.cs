using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleEnemy : Entity, IMoving, IHealth
{
    [SerializeField]
    GameObject guardedObject;
    [SerializeField]
    float speed;


    public float Speed { get { return speed; } set { speed = value; } }
    public Vector2 Velocity { get; set; }
    public int Health { get; set; }

    enum SideOfBox
    {
        Left,
        Right,
        Top,
        Bot
    }
    
    SideOfBox side;

    public void TakeDamage(int amount)
    {
        Health -= amount;
    }

    public void Move()
    {
        RB.velocity = transform.right * Speed; 
    }

    protected override void Start()
    {
        base.Start();
        DetermineInitialSide();
    }

    // Update is called once per frame
    void Update()
    {
        BoxCollider2D guardedCollider = guardedObject.GetComponent<BoxCollider2D>();
        float yRotation;
        switch (side)
        {
            case SideOfBox.Top:
                yRotation = 0;
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, yRotation);
                transform.position = new Vector2(transform.position.x, guardedCollider.bounds.extents.y + guardedObject.transform.position.y);
                if (transform.position.x > guardedCollider.bounds.extents.x + guardedObject.transform.position.x && Speed > 0)
                {
                    
                    side = SideOfBox.Right;                   
                    
                }
                
                if (transform.position.x < -guardedCollider.bounds.extents.x + guardedObject.transform.position.x && Speed < 0)
                {
                    side = SideOfBox.Left;
                }

                break;

            case SideOfBox.Bot:
                yRotation = -180.0f;
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, yRotation);
                transform.position = new Vector2(transform.position.x, -guardedCollider.bounds.extents.y + guardedObject.transform.position.y);
                if (transform.position.x > guardedCollider.bounds.extents.x + guardedObject.transform.position.x && Speed < 0)
                {
                    side = SideOfBox.Right;
                    
                }
                
                if (transform.position.x < -guardedCollider.bounds.extents.x + guardedObject.transform.position.x && Speed > 0)
                {
                    side = SideOfBox.Left;
                    
                }
                break;

            case SideOfBox.Left:
                yRotation = 90.0f;
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, yRotation);
                transform.position = new Vector2(guardedObject.transform.position.x - guardedCollider.bounds.extents.x, transform.position.y);
                if (transform.position.y > guardedCollider.bounds.extents.y + guardedObject.transform.position.y && Speed > 0)
                {
                    side = SideOfBox.Top;
                    
                }
                
                if (transform.position.y < -guardedCollider.bounds.extents.y + guardedObject.transform.position.y && Speed < 0)
                {
                    side = SideOfBox.Bot;
                    
                }
                break;

            case SideOfBox.Right:
                yRotation = -90.0f;
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, yRotation);
                transform.position = new Vector2(guardedObject.transform.position.x + guardedCollider.bounds.extents.x, transform.position.y);
                if (transform.position.y > guardedCollider.bounds.extents.y + guardedObject.transform.position.y && Speed < 0)
                {
                    side = SideOfBox.Top;
                    
                }
            
                if (transform.position.y < -guardedCollider.bounds.extents.y + guardedObject.transform.position.y && Speed > 0)
                {
                    side = SideOfBox.Bot;
                    
                }
                break;
        } 
    }

    void DetermineInitialSide()
    {
        float yRot = transform.rotation.z;
        if (yRot == 90)
            side = SideOfBox.Left;
        if (yRot == -90)
            side = SideOfBox.Right;
        if (yRot == -180)
            side = SideOfBox.Bot;
        if (yRot == 0)
            side = SideOfBox.Top;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag == "Player")
        {
            collision.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 2) + (Vector2)transform.right * speed;
        }
    }

    
}
