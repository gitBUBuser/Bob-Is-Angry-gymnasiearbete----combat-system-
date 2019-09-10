using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    PlayerController controller;

    void Start()
    {
        controller = GetComponent<PlayerController>();
    }
}
