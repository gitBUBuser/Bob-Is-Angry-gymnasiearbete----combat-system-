using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthbar : MonoBehaviour
{
    int maxHealth;
    int currentHealth;
    PlayerController player;

    [SerializeField]
    GameObject[] bigBars;
    [SerializeField]
    GameObject[] smallBars;
    [SerializeField]
    Sprite[] faces;

    SpriteRenderer faceRender;


    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        maxHealth = player.maxHealth;
        faceRender = transform.Find("Face").GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        currentHealth = player.currentHealth;
        float healthPercentage = HealthPercentage();

        

        if(healthPercentage < 100)
        {
            if (healthPercentage < 95)
                smallBars[9].SetActive(false);
            bigBars[9].SetActive(false);
        }
        if (healthPercentage <= 90)
        {
            if (healthPercentage < 85)
                smallBars[8].SetActive(false);
               
            bigBars[8].SetActive(false);
            faceRender.sprite = faces[8];
        }
        if (healthPercentage <= 80)
        {
            if (healthPercentage < 75)
                smallBars[7].SetActive(false);
            bigBars[7].SetActive(false);
            faceRender.sprite = faces[7];
        }
        if (healthPercentage <= 70)
        {
            if (healthPercentage < 65)
                smallBars[6].SetActive(false);
            bigBars[6].SetActive(false);
            faceRender.sprite = faces[6];
        }
        if (healthPercentage <= 60)
        {
            if (healthPercentage < 55)
                smallBars[5].SetActive(false);
            bigBars[5].SetActive(false);
            faceRender.sprite = faces[5];
        }
        if (healthPercentage <= 50)
        {
            if (healthPercentage < 45)
                smallBars[4].SetActive(false);
            bigBars[4].SetActive(false);
            faceRender.sprite = faces[4];
        }
        if (healthPercentage <= 40)
        {
            if (healthPercentage < 35)
                smallBars[3].SetActive(false);
            bigBars[3].SetActive(false);
            faceRender.sprite = faces[3];
        }
        if (healthPercentage <= 30)
        {
            if (healthPercentage <= 25)
                smallBars[2].SetActive(false);
            bigBars[2].SetActive(false);
            faceRender.sprite = faces[2];
        }
        if (healthPercentage <= 20)
        {
            if (healthPercentage < 15)
                smallBars[1].SetActive(false);
            bigBars[1].SetActive(false);
            faceRender.sprite = faces[1];
        }
        if (healthPercentage <= 10)
        {
            faceRender.sprite = faces[1];
            if (healthPercentage <= 0)
            {
                faceRender.sprite = faces[0];
                smallBars[0].SetActive(false);
            }
            bigBars[0].SetActive(false);
        }
    }

    float HealthPercentage()
    {
        return ((float)currentHealth / (float)maxHealth) * 100f;
    }
}
