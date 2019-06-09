using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeForce : MonoBehaviour {

    Rigidbody rb;
    [SerializeField] float speed = 2;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip absorbAudio;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponentInChildren<AudioSource>();
    }

    // Use this for initialization
    void Start () {
	    	
	}
	
	// Update is called once per frame
	void Update () {
        Player p = FindObjectOfType<Player>();
        if (p == null) return;
        transform.LookAt(p.transform);
        rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Force);
        float dist = Vector3.Distance(transform.position, p.transform.position);
        if (dist < 0.5f)
        {
            p.PlayAbsorbClip();
            //audioSource.PlayOneShot(absorbAudio);
            Destroy(gameObject);
            var gm = FindObjectOfType<GameManager>();
            if (gm != null)
            {
                gm.AddSoul();
            }
        }
	}
}
