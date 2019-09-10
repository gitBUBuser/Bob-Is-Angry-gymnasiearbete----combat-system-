using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    int health;

    public void TakeDamage(int amount)
    {
        health -= amount;
    }
}
