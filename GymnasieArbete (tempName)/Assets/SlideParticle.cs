using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideParticle : MonoBehaviour
{
    public Transform Follow { get; set; }
    public bool Following { get; set; } = true;
 
    private void Update()
    {
        if (Following)
            transform.position = GameObject.Find("SlideT").transform.position;
    }
}
