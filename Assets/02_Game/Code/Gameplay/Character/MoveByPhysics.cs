using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlobbInvasion.Gameplay.Character
{
    //S: Move the game object into a direction by using a rigid body
    //      That either happens by force or velocity
    [RequireComponent(typeof(Rigidbody2D))]
    public class MoveByPhysics : MonoBehaviour, IMoveable
    {

        [SerializeField]
        private float MovementSpeed;
        [SerializeField]
        private bool MoveByForce;
        [SerializeField]
        private float MaxSpeed;

        public const float POINT_STOPPING_DISTANCE = 0.1f;

        private Rigidbody2D mPhysicsBody;
        private Vector2 mCurrentDirection;
        private Vector2 mCurrentTarget;
        private bool mMoveToTarget = false;

        void Start()
        {
            mPhysicsBody = GetComponent<Rigidbody2D>();
        }

        void FixedUpdate()
        {
            if (MoveByForce)
            {
                if(mMoveToTarget) throw new NotSupportedException("Moving to target currently only supported for move by velocity");

                mPhysicsBody.AddForce(mCurrentDirection * MovementSpeed);

                // define max speed
                if (mPhysicsBody.velocity.magnitude > MaxSpeed)
                {
                    mPhysicsBody.velocity = mPhysicsBody.velocity.normalized * MaxSpeed;
                }

            }
            else
            {

                bool reachedTarget = false;
                if(mMoveToTarget)
                {
                    float distanceToTarget = Vector2.Distance(transform.position,mCurrentTarget);
                    if(distanceToTarget <= POINT_STOPPING_DISTANCE)
                    {
                        mPhysicsBody.velocity = Vector2.zero;
                        reachedTarget = true;
                    }

                }
     
                if(!reachedTarget)
                    mPhysicsBody.velocity = mCurrentDirection * MovementSpeed;
            }
        }

        public void Move(Vector2 direction)
        {
            direction.Normalize();
            mCurrentDirection = direction;
            mMoveToTarget = false;
        }

        public void MoveFaster(Vector2 direction, float multiplicator)
        {
            direction.Normalize();
            mCurrentDirection = direction * multiplicator;
            mMoveToTarget = false;
        }

        public void MoveTo(Vector2 position)
        {
            Vector2 direction = position - (Vector2)transform.position;
            direction.Normalize();
            mCurrentDirection = direction;
            mCurrentTarget = position;
            mMoveToTarget = true;
        }
    }
}