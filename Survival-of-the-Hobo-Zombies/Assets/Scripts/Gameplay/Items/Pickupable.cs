using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : Interactable
{
    public bool autopickup = false;

    public override void Interact()
    {
        DestroyImmediate(gameObject);
    }   

    void Update()
    {
        if (autopickup)
        {
            Player player = GameObject.FindObjectOfType<Player>();
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < 4f)
            {
                Rigidbody rigBody = GetComponent<Rigidbody>();
                if (rigBody != null)
                {
                    rigBody.AddForce((player.transform.position - transform.position) * (1 - distance/4) * 500 * Time.smoothDeltaTime);
                }
            }

            if (distance < 1.5f)
            {
                Interact(player);
            }
        }
    }
}
