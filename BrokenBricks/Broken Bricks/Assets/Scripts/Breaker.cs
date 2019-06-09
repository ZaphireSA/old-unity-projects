using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Breaker : MonoBehaviour
{
    public float speed = 3.0F;
    Rigidbody rBody;

    public float movementDamp = 0.3f;
    public GameObject breakerObject;

    GameObject leftLimit;
    GameObject rightLimit;

    [SerializeField]
    GameObject aimObject;
    [SerializeField]
    GameObject targetObject;

    public Transform ballHolder;
    private Vector3 originalSize;

    public GameObject breakerVisual;

    public float targetX = 0;


    public enum BreakerState
    {
        Aim,
        Move
    }

    public BreakerState state = BreakerState.Aim;

    void Awake()
    {
        rBody = GetComponent<Rigidbody>();
        leftLimit = GameObject.Find("LeftLimit");
        rightLimit = GameObject.Find("RightLimit");
    }

    private void Start()
    {
        ChangeState(BreakerState.Aim);
        originalSize = breakerVisual.transform.localScale;
    }

    public void ChangeState(BreakerState newState)
    {
        state = newState;
        aimObject.SetActive(newState == BreakerState.Aim);
    }

    public Vector3 GetTargetPosition()
    {
        return targetObject.transform.position;
    }

    void Update()
    {
        if (Time.timeScale == 0) return;
        //Vector3 sideways = transform.TransformDirection(Vector3.right);

        Vector3 targetPos = transform.position;

        if (state == BreakerState.Aim)
        {
            targetPos = targetObject.transform.position;
        }

#if UNITY_ANDROID && !UNITY_EDITOR
        if (state == BreakerState.Move) targetPos.x = targetX;


        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                targetPos.x = Camera.main.ScreenToWorldPoint(touch.position).x;
            }
        }

        //foreach (var touch in Input.touches)
        //{
        //    if (touch.phase == TouchPhase.Moved)
        //    {
        //        targetPos.x = Camera.main.ScreenToWorldPoint(touch.position).x;
        //    }
        //}

#else
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPos.x = pos.x;
#endif        
        if (state == BreakerState.Aim)
        {
            var newPos = targetObject.transform.position;
            //newPos.x = targetPos.x;
            newPos.x = Mathf.Clamp(targetPos.x, leftLimit.transform.position.x, rightLimit.transform.position.x);
            targetObject.transform.position = newPos;
            aimObject.transform.LookAt(targetObject.transform);

        }


        if (state != BreakerState.Move)
        {
            var scale2 = breakerVisual.transform.localScale;
            scale2.z = originalSize.x;
            breakerVisual.transform.localScale = scale2;
            return;
        }


        float widthOfBreaker = breakerObject.GetComponent<Collider>().bounds.extents.x;
        Vector3 difference = transform.position - targetPos;

        var scale = breakerVisual.transform.localScale;
        scale.z = Mathf.Clamp(originalSize.x - Mathf.Abs(difference.x), 0.5f, originalSize.z);
        breakerVisual.transform.localScale = scale;

        targetX = targetPos.x;

        targetPos.x = Mathf.Clamp(targetPos.x, leftLimit.transform.position.x + widthOfBreaker, rightLimit.transform.position.x - widthOfBreaker);

        targetPos = Vector3.Lerp(transform.position, targetPos, movementDamp);


        transform.position = targetPos;

        //if ((transform.position.x + widthOfBreaker < rightLimit.transform.position.x || curSpeed < 0) && (transform.position.x - widthOfBreaker > leftLimit.transform.position.x || curSpeed > 0))
        //if ((transform.position.x + widthOfBreaker < rightLimit.transform.position.x || difference.x > 0) && (transform.position.x - widthOfBreaker > leftLimit.transform.position.x || difference.x < 0))
        //{
        //    transform.position = targetPos;
        //    //transform.Translate (sideways * curSpeed * Time.deltaTime);
        //}
    }
}
