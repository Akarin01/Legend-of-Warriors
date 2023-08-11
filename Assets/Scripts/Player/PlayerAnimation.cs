using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private const string speedXParam = "speedX";
    private const string velocityYParam = "velocityY";
    private const string isGroundParam = "isGround";

    private Animator animator;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
    }

    private void Update()
    {
        SetAnimation();
    }

    private void SetAnimation()
    {
        animator.SetFloat(speedXParam, Mathf.Abs(rb.velocity.x));
        animator.SetFloat(velocityYParam, rb.velocity.y);
        animator.SetBool(isGroundParam, physicsCheck.IsGround);
    }
}
