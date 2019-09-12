using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField]
    int RA_dmg,
        Basic_dmg,
        Heavy_dmg;

    List<Collider2D> collisions;


    void Start()
    {
        collisions = new List<Collider2D>();
    }

    private void OnDisable()
    {
        ResetWeapon();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {  
        if (!collisions.Contains(collision))
        {
            if (collision.transform.tag == "Chunk")
            {
                Vector2 distance = collision.transform.position - transform.position;
                collision.GetComponent<Rigidbody2D>().AddForce(distance.normalized * 0.0001f, ForceMode2D.Impulse);
            }
            if (collision.GetComponent(typeof(IHealth)))
            {
                collision.GetComponent<IHealth>().TakeDamage(RA_dmg);
            }
            collisions.Add(collision);
        }      
    }

    public void ResetWeapon()
    {
        collisions.Clear();
    }
}
