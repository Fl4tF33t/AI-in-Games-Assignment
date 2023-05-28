using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using JetBrains.Annotations;

public class MoveableBox : MonoBehaviour
{
    public event EventHandler OnTriggerReached;

    public float speed;
    Rigidbody rb;
    Vector3 target;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveBoxToTarget(Target(target) * speed);
        if(transform.position.y < 1 && target != Vector3.zero)
        {
            target = Vector3.zero;
            OnTriggerReached?.Invoke(this, EventArgs.Empty);
        }
    }

    private void MoveBoxToTarget(Vector3 target)
    {
        if(target == null)
        {
            target = Vector3.zero;
        }
        rb.AddForce(target);
    }

    public Vector3 Target(Vector3 Target)
    {
        target = Target;
        return target;
    }
}
