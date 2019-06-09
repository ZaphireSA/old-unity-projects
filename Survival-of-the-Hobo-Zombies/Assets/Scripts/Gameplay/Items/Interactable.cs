using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

    public Material normalMat;
    public Material highlightMat;
    public bool isInteractable = true;

    public virtual void OnMouseOver()
    {
        if (highlightMat == null || normalMat == null) return;

        if (!isInteractable)
        {
            GetComponentInChildren<Renderer>().material = normalMat;
            return;
        }

        if (Vector3.Distance(transform.position, Camera.main.transform.position) < 7f)
        {
            GetComponentInChildren<Renderer>().material = highlightMat;
        }
        else
        {
            GetComponentInChildren<Renderer>().material = normalMat;
        }
    }

    public virtual void OnMouseExit()
    {
        if (normalMat == null) return;
        GetComponentInChildren<Renderer>().material = normalMat;
    }

    public virtual void Interact()
    {

    }

    public virtual void Interact(Player player)
    {

    }

    public virtual void Interact(Vector3 position)
    {

    }
}
