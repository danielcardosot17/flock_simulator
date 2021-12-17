using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Range(1f,10f)] public float moveSpeed = 5;

    [Range(1f,3f)] public float turboMultiplier = 1.2f;

    private Vector3 direction = Vector3.zero;
    private float multiplier = 1;

    // Update is called once per frame
    void Update()
    {
        direction = Vector3.right * Input.GetAxis("Horizontal") + Vector3.up * Input.GetAxis("Vertical");
        multiplier = Input.GetKey(KeyCode.Space) ? turboMultiplier : 1;
        if(direction.magnitude > 0) Move(direction, multiplier);    
    }

    private void Move(Vector3 direction, float multiplier)
    {
        transform.up = direction.normalized;
        transform.position += direction.normalized * multiplier * moveSpeed * Time.deltaTime;
    }
    public LayerMask obstacleLayer;
    // Start is called before the first frame update
    private void OnCollisionStay(Collision other) {
        if(obstacleLayer == (obstacleLayer | 1 << other.gameObject.layer))
        {
            if(direction.magnitude > 0)
            {
                Vector3 closestPoint = other.gameObject.GetComponent<Collider>().ClosestPoint(transform.position);
                var normalVector = (transform.position - closestPoint).normalized;
                Debug.Log(normalVector);
                Move(normalVector, multiplier);
            } 
        }
    }
}
