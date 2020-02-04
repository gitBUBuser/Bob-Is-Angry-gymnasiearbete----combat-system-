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
    [SerializeField] float radiusSlide;

    [Header("Attack Position")]
    [SerializeField] Transform pointSideAir;
    [SerializeField] Transform pointDownAir;
    [SerializeField] Transform pointNeutralAir;
    [SerializeField] Transform pointSide;
    [SerializeField] Transform pointSlide;

    [Header("Other Variables")]
    [SerializeField] float downAirPushback;
    [SerializeField] float slideKnockup;

    int sideAirIndex;
    bool dealtDamage;
    PlayerController controller;
    PlayerFX fx;
    List<Entity> hitEntities;

    enum AttackType
    {
        Side,
        SAir,
        DAir,
        UAir
    }

    AttackType type;

    private void Start()
    {
        hitEntities = new List<Entity>();
        controller = GetComponent<PlayerController>();
        fx = GetComponent<PlayerFX>();
    }

    public void SideAir()
    {
        type = AttackType.SAir;
        Collider2D[] results = CollidersWithinCircle(pointSideAir, radiusSideAir);
        for (int i = 0; i < results.Length; i++)
        {
            if(sideAirIndex == 0)
            {
                DealKnockbackIfTarget(new Vector2(3 * transform.right.x, 0), results[i]);
            }
            else
            {
                DealKnockbackIfTarget(new Vector2(5 * transform.right.x,8), results[i]);
            }
            DealDamageIfTarget(dmgSideAir, results[i]);
            ParticleIfTarget(results[i]);
        }
        sideAirIndex++;
        ResetVariables();
    }

    public void Side()
    {
        type = AttackType.Side;
        Collider2D[] results = CollidersWithinCircle(pointSideAir, radiusSideAir);

        for (int i = 0; i < results.Length; i++)
        {
            DealKnockbackIfTarget(new Vector2(7 * transform.right.x, 4), results[i]);
            DealDamageIfTarget(dmgSide, results[i]);
            ParticleIfTarget(results[i]);
        }
    }

    public void Slide()
    {
        Collider2D[] results = CollidersWithinCircle(pointSlide, radiusSlide);

        for (int i = 0; i < results.Length; i++)
        {
            DealKnockbackIfTarget(Vector2.up * slideKnockup, results[i]);

            if (results[i].GetComponent<Entity>())
                hitEntities.Add(results[i].GetComponent<Entity>());
        }
        ResetVariables();
    }

    public void DownAir()
    {
        type = AttackType.DAir;
        Collider2D[] results = CollidersWithinCircle(pointDownAir, radiusDownAir);

        for (int i = 0; i < results.Length; i++)
        {
           // DealKnockbackIfTarget(Vector2.down * 2, results[i]);
            DealDamageIfTarget(dmgDownAir, results[i]);
        }
        if (dealtDamage)
            controller.Jump(downAirPushback);
        ResetVariables();
    }

    public void NeutralAir()
    {
        type = AttackType.UAir;
        Collider2D[] results = CollidersWithinCircle(pointNeutralAir, radiusNeutralAir);

        for (int i = 0; i < results.Length; i++)
        {
            DealDamageIfTarget(dmgNeutralAir, results[i]);
            DealKnockbackIfTarget(Vector2.up * 8, results[i]);
        }
        ResetVariables();
    }

    void DealDamageIfTarget(int damage, Collider2D collider)
    {
        if (collider.GetComponent<Entity>() && collider.gameObject != gameObject)
        {
            Time.timeScale = 0.1f;
         //   collider.GetComponent<IHealth>().TakeDamage(damage);
            StartCoroutine(LerpTimeBack(0.2f));
            dealtDamage = true;
        }
    }

    IEnumerator LerpTimeBack(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        Time.timeScale = 1;
    }

    void DealKnockbackIfTarget(Vector2 knockback, Collider2D collider)
    {
        if (collider.GetComponent<Entity>() && collider.gameObject != gameObject)
        {
            if (!hitEntities.Contains(collider.GetComponent<Hand>()))
            {

                collider.GetComponent<Rigidbody2D>().velocity = knockback;
                collider.GetComponent<GroundedEnemy>().Stun();
            }
        }
    }

    void ParticleIfTarget(Collider2D collider)
    {     
        if (collider.GetComponent<Entity>() && collider.gameObject != gameObject)
        {
            switch (type)
            {
                case AttackType.SAir:
                    fx.BloodSplatSair(collider.transform,pointSideAir.position, sideAirIndex);
                    break;

                case AttackType.Side:
                    fx.BloodSplatSide(collider.transform.position);
                    break;
            }
        }
    }

    public void ResetList()
    {
        hitEntities.Clear();
    }

    public void SideAirIsDone()
    {
        sideAirIndex = 0;
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
        Gizmos.DrawWireSphere(pointSlide.position, radiusSlide);

    }
}
