using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadController : MonoBehaviour {

    Rigidbody rb;
    [SerializeField] float rollSpeed = 5f;
    Vector3 velocity = Vector3.zero;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        var vertical = Input.GetAxis("Vertical");
        var horizontal = Input.GetAxis("Horizontal");
        velocity = new Vector3(horizontal, 0, vertical);

        //velocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        rb.AddForce(velocity.normalized * rollSpeed * Time.deltaTime);
    }
}
