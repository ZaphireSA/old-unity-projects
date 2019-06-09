using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;
    public float smoothTime = 0.3f;
    [SerializeField]
    Vector3 offset = Vector3.zero;

    private Vector3 velocity = Vector3.zero;
    private void Start()
    {
        offset.y = transform.position.y;
    } 

    void Update()
    {
        if (target == null) return;
        Vector3 goalPos = target.position + offset;
        //goalPos.y = transform.position.y;
        transform.position = Vector3.SmoothDamp(transform.position, goalPos, ref velocity, smoothTime);
    }
}
