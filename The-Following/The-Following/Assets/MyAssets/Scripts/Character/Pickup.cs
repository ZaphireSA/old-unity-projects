using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

    [SerializeField]
    int amount;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip clip;

    enum Type
    {
        Protein
    }

    private void Awake()
    {
        audioSource = GetComponentInChildren<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();
        if (player != null)
        {
            player.PlayPickupClip();
            player.GiveProtein(amount);
            Destroy(transform.parent.gameObject);
        }
    }

}
