using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerController))]
public class Player : LivingEntity {

    public enum State { Active, Jumping }
    public State state = State.Active;

    PlayerController controller;
    public WeaponController weaponController { get; private set; }
   

    public float speedSmoothTime = 0.1f;
    public float speedSmoothVelocity;
    public float currentSpeed;

    public bool isZoomedIn = false;

    public Crosshairs crosshair;

    protected override void Start()
    {
        base.Start();
        crosshair = GameObject.FindObjectOfType<Crosshairs>();
    }

    void Awake()
    {
        controller = GetComponent<PlayerController>();
        weaponController = GetComponent<WeaponController>();
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, 1.5f);
    }

    void Update()
    {
        //Movement Input
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        bool isGrounded = IsGrounded();
        if (state != State.Active)
        {
            moveInput = new Vector3(0, 0, 0);
        }
        moveInput = transform.TransformDirection(moveInput);

        Vector3 animateInput = transform.InverseTransformDirection(moveInput);
        anim.SetFloat("VelocityX", animateInput.x);
        anim.SetFloat("VelocityZ", animateInput.z);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            controller.Jump(jumpSpeed);
        }

        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        float targetSpeed = ((isRunning) ? runSpeed : walkSpeed) * moveInput.normalized.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        Vector3 moveVelocity = moveInput.normalized * currentSpeed;

        controller.Move(moveVelocity);        

        float animationSpeedPercent = ((isRunning) ? 1 : 0.5f) * moveInput.normalized.magnitude;
        anim.SetFloat("SpeedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);

        RaycastHit hit;
        if (Physics.Raycast(controller.m_Cam.position, controller.m_Cam.forward, out hit, 500,  ~(1 << LayerMask.NameToLayer("Player"))))
        {
            weaponController.Aim(hit.point);
        } else
        {
            weaponController.Aim(controller.m_Cam.GetComponentInParent<FreeLookCam>().aimPoint.position);
        }


        if (Input.GetKeyDown(KeyCode.E) && state == State.Active)
        {
            RaycastHit itemHit;
            Ray itemRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(itemRay, out itemHit))
            {
                if (itemHit.collider.GetComponentInParent<Interactable>() != null)
                {
                    if (Vector3.Distance(controller.m_Cam.transform.position, itemHit.transform.position) < 7f)
                    {
                        itemHit.collider.GetComponentInParent<Interactable>().Interact(this);
                    }
                }
            }
        }


        if (Input.GetMouseButton(0) && state == State.Active)
        {
            weaponController.OnTriggerHold();
        }

        if (Input.GetMouseButtonUp(0))
        {
            weaponController.OnTriggerRelease();
        }

        if (Input.GetMouseButtonDown(1))
        {
            isZoomedIn = true;
        }

        if (Input.GetMouseButtonUp(1))
        {
            isZoomedIn = false;
        }

        if (Input.GetKeyDown(KeyCode.G) && state == State.Active)
        {
            weaponController.DropWeapon();
        }

        if (Input.GetKeyDown(KeyCode.R) && state == State.Active)
        {
            weaponController.Reload();
        }

        if (Input.GetKeyDown(KeyCode.Q) && state == State.Active)
        {
            weaponController.SwitchBetweenWeapons();
        }

    }

    public override void TakeDamage(float damage, bool isCrit)
    {
        base.TakeDamage(damage, isCrit);
        anim.SetTrigger("GetHit");
    }

    void LateUpdate()
    {
        if (headBone != null)
        {
            headBone.LookAt(controller.m_Cam.GetComponentInParent<FreeLookCam>().aimPoint.position);
        }
    }

}
