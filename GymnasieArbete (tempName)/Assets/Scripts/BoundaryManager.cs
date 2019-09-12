using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryManager : MonoBehaviour
{
    BoxCollider2D managerBox;
    Transform player;
    GameObject boundary;

    private void Start()
    {
        managerBox = GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        
    }

    void ManageBoundary()
    {

    }
}
