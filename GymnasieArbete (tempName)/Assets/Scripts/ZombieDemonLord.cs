using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDemonLord : Entity, IMoving, IHealth
{
    public float Speed { get; set; }
    public Vector2 Velocity { get; set; }

    public int Health { get; set; }

    [SerializeField]
    Transform headT,
        torsoT,
        legsT;

    [SerializeField]
    GameObject[] chunks;

    [SerializeField]
    int health;
    float shockForce = 1.6f;

    public void Move()
    {
        RigidBody.velocity = Velocity;
    }

    public void TakeDamage(int amount)
    {
        Debug.Log(gameObject + "|" + amount);

        Health -= amount;
        RigidBody.velocity = new Vector2(RigidBody.velocity.x, shockForce);
        if(Health < 0)
        {
            Die();
        }
    }

    void Die()
    {
        for (int i = 0; i < chunks.Length; i++)
        {
            chunks[i].SetActive(true);
            chunks[i].transform.parent = null;
            chunks[i].GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1, 1) * 0.0001f, Random.Range(1, 2) * 0.0001f), ForceMode2D.Impulse);
        }
    }


    protected override void Start()
    {
        base.Start();
        Health = 10;
    }

    // Update is called once per frame
    void Update()
    {
        health = Health;
    }
}
