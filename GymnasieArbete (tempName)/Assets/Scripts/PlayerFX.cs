using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFX : MonoBehaviour
{
    [SerializeField]
    GameObject runParticle;

    [SerializeField]
    Transform feetPosition;

    public void RunParticle()
    {
        Destroy(Instantiate(runParticle, feetPosition.position, transform.rotation), 1f);
    }
}
