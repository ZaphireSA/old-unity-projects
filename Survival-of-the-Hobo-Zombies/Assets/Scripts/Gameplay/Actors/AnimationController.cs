using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour {

    public Animator animator;
    public float speedSmoothTime = 0.1f;
    public float speedSmoothVelocity;
    public float currentSpeed;

    void Update()
    {

    }

    public void UpdateMovement(Vector3 dir, bool isRunning)
    {

        //animator.SetFloat("speedPercent", speedPercent);
    }
}
