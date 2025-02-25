﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MoveByPhysics : MonoBehaviour, IMoveable
{

    [SerializeField]
    private float MovementSpeed;
    [SerializeField]
    private bool MoveByForce;
    [SerializeField]
    private float MaxSpeed;

    private Rigidbody2D mPhysicsBody;
    private Vector2 mCurrentDirection;

    void Start()
    {
        mPhysicsBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if(MoveByForce)
        {
           mPhysicsBody.AddForce(mCurrentDirection * MovementSpeed);

            // define max speed
           if(mPhysicsBody.velocity.magnitude > MaxSpeed)
           {
               mPhysicsBody.velocity = mPhysicsBody.velocity.normalized * MaxSpeed;
           }

        }
        else
        {
            mPhysicsBody.velocity = mCurrentDirection * MovementSpeed;
        }
    }

    public void Move(Vector2 direction)
    {
        direction.Normalize();
        mCurrentDirection = direction;

    }
}
