using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenWall : MonoBehaviour {

    Cube player;

    private void Start()
    {
        player = FindObjectOfType<Cube>();
    }

    void FixedUpdate () {
        if (transform.position.x <= player.transform.position.x - 30)
        {
            Destroy(gameObject);
        }
    }
}
