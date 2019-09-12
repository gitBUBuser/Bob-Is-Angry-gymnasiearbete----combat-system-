using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    int Health { get; set; }
    void TakeDamage(int amount);
}

