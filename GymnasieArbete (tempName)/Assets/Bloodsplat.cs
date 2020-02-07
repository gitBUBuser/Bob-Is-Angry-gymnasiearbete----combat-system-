using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloodsplat : MonoBehaviour
{
    [SerializeField]
    GameObject bloodSplatter;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("LOLE!");
        Instantiate(bloodSplatter, collision.GetContact(0).point, Quaternion.identity);
    }
}
