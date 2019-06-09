using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

    public bool isMoving = false;
    public float speed = 2f;
    public float minSpeed = 1f;
    public float maxSpeed = 6f;
    public float moveDistance = 4f;
    public float minDistance = 1f;
    public float maxDistance = 8f;

    Vector3 originalPos = Vector3.zero;

    void FixedUpdate()
    {
        if (isMoving)
            transform.position = new Vector3(originalPos.x, originalPos.y + (Mathf.Sin(Time.time * speed) * moveDistance), originalPos.z);
    }

    public void UpdateState(bool moving)
    {
        isMoving = moving;
        originalPos = transform.position;
        moveDistance = Random.Range(minDistance, maxDistance);
        speed = Random.Range(minSpeed, maxSpeed);
    }

    public void UpdateOriginalPos(float yPos)
    {
        originalPos.y += yPos;
    }
}
