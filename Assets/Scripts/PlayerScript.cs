using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Rigidbody rb;
    public float maxSpeed = 5f;
    public Animator animator;
    public Vector3 target = Vector3.zero;
    public SpriteRenderer sprite;
    Vector2 lastDir = Vector2.zero;

    // Update is called once per frame
    void Update()
    {
        Vector3 velocity = rb.velocity;

        if (velocity.sqrMagnitude > 0.01)
        {
            lastDir = velocity.normalized;
        }

        sprite.flipX = lastDir.x < 0.01;

        animator.SetFloat("LastHori", lastDir.x);
        animator.SetFloat("LastVerti", lastDir.y);

        animator.SetFloat("Horizontal", velocity.x);
        animator.SetFloat("Vertical", velocity.z);
        animator.SetFloat("Speed", velocity.sqrMagnitude);
    }

    void FixedUpdate()
    {
        //movement
        Vector3 desired = target - rb.position;
        
        rb.velocity = desired * maxSpeed;
    }

    public void movePlayerToTarget(Vector3 newTarget) 
    {
        target = newTarget;
    }
}
