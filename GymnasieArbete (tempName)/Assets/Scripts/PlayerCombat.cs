using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
   

    [Header("Attack Damage")]
    [SerializeField] int dmgSideAir;
    [SerializeField] int dmgDownAir;
    [SerializeField] int dmgNeutralAir;
    [SerializeField] int dmgSide;

    [Header("Attack Radius")]
    [SerializeField] float radiusSideAir;
    [SerializeField] float radiusDownAir;
    [SerializeField] float radiusNeutralAir;
    [SerializeField] float radiusSide;

    [Header("Attack Position")]
    [SerializeField] Transform pointSideAir;
    [SerializeField] Transform pointDownAir;
    [SerializeField] Transform pointNeutralAir;
    [SerializeField] Transform pointSide;

    [Header("Other Variables")]
    [SerializeField] float downAirPushback;

    bool dealtDamage;
    PlayerController controller;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
    }

    public void SideAir()
    {
        Collider2D[] results = CollidersWithinCircle(pointSideAir, radiusSideAir);

        for (int i = 0; i < results.Length; i++)
        {
            DealKnockbackIfTarget(new Vector2(10, 4), results[i]);
            DealDamageIfTarget(dmgSideAir, results[i]);
        }
        ResetVariables();
    }

    public void DownAir()
    {
        Collider2D[] results = CollidersWithinCircle(pointDownAir, radiusDownAir);

        for (int i = 0; i < results.Length; i++)
        {
            DealDamageIfTarget(dmgDownAir, results[i]);
        }
        if (dealtDamage)
            controller.Jump(downAirPushback);
        ResetVariables();
    }

    public void NeutralAir()
    {
        Collider2D[] results = CollidersWithinCircle(pointSideAir, radiusSideAir);

        for (int i = 0; i < results.Length; i++)
        {
            DealDamageIfTarget(dmgNeutralAir, results[i]);
        }
        ResetVariables();
    }

    void DealDamageIfTarget(int damage, Collider2D collider)
    {
        if (collider.GetComponent<Entity>() && collider.gameObject != gameObject)
        {
            collider.GetComponent<IHealth>().TakeDamage(damage);
            Debug.Log("Dealt damage 2: " + collider.gameObject.name);
            dealtDamage = true;
        }
    }

    void DealKnockbackIfTarget(Vector2 knockback, Collider2D collider)
    {
        if (collider.GetComponent<Entity>() && collider.gameObject != gameObject)
        {
            Debug.Log("Dealt knockback 2: " + collider.gameObject.name);
            collider.GetComponent<Entity>().AddForce(new Vector2(knockback.x * transform.right.x, knockback.y));
        }
    }

    void ResetVariables()
    {
        dealtDamage = false;
    }

    Collider2D[] CollidersWithinCircle(Transform position, float radius)
    {
        return Physics2D.OverlapCircleAll(position.position, radius);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pointSideAir.position, radiusSideAir);
        Gizmos.DrawWireSphere(pointNeutralAir.position, radiusNeutralAir);
        Gizmos.DrawWireSphere(pointDownAir.position, radiusDownAir);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(pointSide.position, radiusSide);
    }
}
