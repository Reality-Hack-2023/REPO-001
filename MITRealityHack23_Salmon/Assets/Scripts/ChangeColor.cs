using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    Material material;

    ParticleSystem ps;

    private void Start()
    {
        material = GameObject.Find("tianyige").GetComponent<Renderer>().material;
        ps = GameObject.Find("LowPolyHands").GetComponent<ParticleSystem>();
    }

    //Scene2
    public void ChangeColorEmission()
    {
        material.SetColor("_EmissionColor", Color.Lerp(Color.white, Color.black, (Time.time - 25) * 0.5f));
        //Debug.Log("bing bong" + material.GetColor("_EmissionColor"));
    }

    public void PlayRainAnimation()
    {
        ps.Play();
        ParticleSystem.EmissionModule em = ps.emission;
        em.enabled = true;
       
    }

    public void EndRainAnimation()
    {
        ps.Pause();
        ParticleSystem.EmissionModule em = ps.emission;
        em.enabled = false;
    }



    private void Update()
    {

    }


}
