using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

    Vector3 velocity;
    Rigidbody myRigidbody;


    public Transform m_Cam;                  // A reference to the main camera in the scenes transform
    //private Vector3 m_CamForward;             // The current forward direction of the camera

    public float turnSmoothTime = 0.2f;
    Vector3 turnSmoothVelocity;

    Player player;

    void Awake()
    {
        player = GetComponent<Player>();
    }

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        // get the transform of the main camera
        if (Camera.main != null)
        {
            m_Cam = Camera.main.transform;
        }
        StartCoroutine(RunningNoise());
    }

    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    IEnumerator RunningNoise()
    {
        while(true)
        {
            if (velocity.magnitude > player.walkSpeed + 1f)
            {
                MakeNoise();
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    void MakeNoise()
    {
        EnemyStateManager[] enemies = GameObject.FindObjectsOfType<EnemyStateManager>();
        foreach (EnemyStateManager enemy in enemies)
        {
            enemy.HearSound(transform.position, 2f);
        }
    }

    public void LookAt(Vector3 lookPoint)
    {
        //Vector3 heightCorrectedPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
        
        //transform.LookAt(heightCorrectedPoint);


    }

    public void Jump(float amount)
    {
        myRigidbody.AddForce(transform.up * amount);
    }

    void FixedUpdate()
    {
        myRigidbody.MovePosition(myRigidbody.position + velocity * Time.fixedDeltaTime);
        if (m_Cam != null)
        {
            // calculate camera relative direction to move:
            //m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            transform.rotation = Quaternion.Euler(0, m_Cam.transform.eulerAngles.y, 0);
        }
    }
}
