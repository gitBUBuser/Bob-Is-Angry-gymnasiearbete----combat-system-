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
    GameObject bloodSplatUp;
    [SerializeField]
    GameObject bloodSplatDown;
    [SerializeField]
    GameObject[] bloodSpatSideAir;
    [SerializeField]
    GameObject jumpingParticle;

    [SerializeField]
    GameObject comboText;

    [SerializeField]
    Transform bsSpawnSide,
        bsSpawnUpAir,
        bsSpawnDownAir;

    [SerializeField]
    Transform[] bsSpawnSideAir;

    [SerializeField]
    Transform feetPosition;
    [SerializeField]
    Transform slidePosition;
    [SerializeField]
    Transform comboTextPos;

    [SerializeField]
    GameObject hitParticle;

    ShakeTransform camShake;

    private void Start()
    {
        camShake = Camera.main.GetComponentInParent<ShakeTransform>();
    }

    public void CamShake(ShakeTransformEventData data)
    {
        camShake.AddShakeEvent(data);
    }

    public void ComboParticle()
    {
        GameObject textObject = Instantiate(comboText, comboTextPos.position, Quaternion.identity);
        textObject.GetComponent<ComboCounter>().Init(Camera.main.transform.Find("Canvas").Find("ComboCounter").GetComponent<ComboCounter>().ComboIndex);
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

    public void HitParticle(Vector2 position)
    {
        Instantiate(hitParticle, position, Quaternion.identity);
    }

    public void BloodSplatSide(Vector2 position)
    {
        Instantiate(bloodSplatSideParticle, bsSpawnSide.position, transform.rotation);
        HitParticle(bsSpawnSide.position);
    }

    public void BloodSplatDown(Vector2 position)
    {
        Instantiate(bloodSplatDown, bsSpawnDownAir.position, transform.rotation);
        HitParticle(bsSpawnDownAir.position);
    }

    public void BloodSplatUp(Vector2 position)
    {
        Instantiate(bloodSplatUp, bsSpawnUpAir.position, transform.rotation);
        HitParticle(bsSpawnUpAir.position);
    }

    public void BloodSplatSair(Transform parent, Vector2 position, int index)
    {
        GameObject test = Instantiate(bloodSpatSideAir[index], bsSpawnSideAir[index].position, transform.rotation);
        HitParticle(bsSpawnSideAir[index].position);
    }

    public void DestroySlideParticle()
    {
      //  GameObject slideP = GameObject.FindGameObjectWithTag("Respawn");
    //    slideP.GetComponent<SlideParticle>().Following = false;
       // slideP.tag = "Untagged";
        //Destroy(slideP, 4f);
    }
}
