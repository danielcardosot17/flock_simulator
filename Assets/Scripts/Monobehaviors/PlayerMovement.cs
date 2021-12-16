using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Range(1f,10f)] public float moveSpeed = 5;

    [Range(1f,3f)] public float turboMultiplier = 1.2f;

    // Update is called once per frame
    void Update()
    {
        var direction = Vector3.right * Input.GetAxis("Horizontal") + Vector3.up * Input.GetAxis("Vertical");
        var multiplier = Input.GetKey(KeyCode.Space) ? turboMultiplier : 1;
        if(direction.magnitude > 0) Move(direction * multiplier);    
    }

    private void Move(Vector3 direction)
    {
        transform.up = direction.normalized;
        transform.position += direction.normalized * moveSpeed * Time.deltaTime;
    }
}
