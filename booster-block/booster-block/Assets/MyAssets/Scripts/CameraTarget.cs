using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour {

    public Transform target;
    
    public Vector3 offset;    

    float originalY = 0;

    private void Start()
    {
        originalY = transform.position.y;
    }

    void FixedUpdate()
    {
        var newPos = target.transform.position + offset;
        newPos.y = originalY;
        transform.position = newPos;
    }
}
