using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFX : MonoBehaviour
{
    [SerializeField]
    GameObject landingParticle;
    [SerializeField]
    GameObject runParticle;
    [SerializeField]
    GameObject slideParticle;
    [SerializeField]
    GameObject bloodSplatSideParticle;
    [SerializeField]
    GameObject[] bloodSpatSideAir;
    [SerializeField]
    GameObject jumpingParticle;

    [SerializeField]
    Transform feetPosition;
    [SerializeField]
    Transform slidePosition;

    ShakeTransform camShake;

    private void Start()
    {
        camShake = Camera.main.GetComponentInParent<ShakeTransform>();
    }

    public void CamShake(ShakeTransformEventData data)
    {
        camShake.AddShakeEvent(data);
    }

    public void RunParticle()
    {
        Instantiate(runParticle, feetPosition.position, transform.rotation);
    }

    public void LandingParticle()
    {
        Instantiate(landingParticle, new Vector2(transform.position.x, feetPosition.position.y), transform.rotation);
    }

    public void JumpingParticle()
    {
        Instantiate(jumpingParticle, new Vector2(transform.position.x, feetPosition.position.y), transform.rotation);
    }

    public void InstSlideParticle()
    {
        //Instantiate(slideParticle, feetPosition.position, transform.rotation);
     //   slideParticle.GetComponent<ParticleSystem>().startRotation3D = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
        //slideParticle.GetComponent<SlideParticle>().Follow = slidePosition;
    }

    public void BloodSplatSide(Vector2 position)
    {
        Instantiate(bloodSplatSideParticle, position, transform.rotation);
    }

    public void BloodSplatSair(Transform parent, Vector2 position, int index)
    {
        GameObject test = Instantiate(bloodSpatSideAir[index], position, new Quaternion(0, transform.rotation.y + 180, 0, 0), parent);
        test.transform.localRotation = new Quaternion(0, 180, 0, 0);
    }

    public void DestroySlideParticle()
    {
      //  GameObject slideP = GameObject.FindGameObjectWithTag("Respawn");
    //    slideP.GetComponent<SlideParticle>().Following = false;
       // slideP.tag = "Untagged";
        //Destroy(slideP, 4f);
    }
}
