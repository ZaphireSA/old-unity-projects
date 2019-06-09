using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Vector3 targetOffset;
	public Transform target;

	public float smoothSpeed = 0.125f;

	void Start() {
		targetOffset = transform.position - target.position;
	}

	void Update() {
		
	}

	void FixedUpdate ()
	{
		Vector3 targetPosition = target.position + targetOffset;

		Vector3 smoothedPosition = Vector3.Lerp (transform.position, targetPosition, smoothSpeed);

		transform.position = smoothedPosition ;

	}
}
