using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoving
{
    float Speed { get; set; }
    Vector2 Velocity { get; set; }
    void Move();
}