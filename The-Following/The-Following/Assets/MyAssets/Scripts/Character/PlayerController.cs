using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    CharacterController controller;
    [SerializeField]
    float rollSpeed = 5f;
    Vector3 velocity = Vector3.zero;
    Animator anim;

    Vector3 moveDirection = Vector3.zero;
    // Use this for initialization

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        anim.SetInteger("Type", 0);
    }

    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 moveDirection = Vector3.zero;
        var vertical = Input.GetAxis("Vertical");
        var horizontal = Input.GetAxis("Horizontal");
        moveDirection = new Vector3(horizontal, 0, vertical);
               
        
        

        //velocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        controller.SimpleMove(moveDirection * rollSpeed * Time.deltaTime);
        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
            anim.SetFloat("SpeedZ", 1f);
            anim.SetBool("IsRunning", true);

        }
        else
        {
            anim.SetFloat("SpeedZ", 0f);
            anim.SetBool("IsRunning", false);

        }
    }
}
