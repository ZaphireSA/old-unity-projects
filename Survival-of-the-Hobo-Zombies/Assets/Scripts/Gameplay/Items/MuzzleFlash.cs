using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour {

    public GameObject flashHolder;
    //public Light[] flashLights;
    public ParticleSystem[] particleSystems;

    public float flashTime = 0.1f;

    void Start()
    {
        Deactivate();
    }

    public void Activate()
    {
        flashHolder.SetActive(true);

        foreach (ParticleSystem ps in particleSystems)
        {
            ps.Play();
        }

        Invoke("Deactivate", flashTime);
    }

    public void Deactivate()
    {
        flashHolder.SetActive(false);
    }


}
