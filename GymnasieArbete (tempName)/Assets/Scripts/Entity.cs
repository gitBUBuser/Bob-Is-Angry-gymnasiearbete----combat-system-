using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    Rigidbody2D rigidBody;

    public Rigidbody2D RigidBody { get { return rigidBody; } }
    public const float Gravity = 30.0f;

    protected virtual void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }
}
