using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator animator;
    PhysicsCheck physicsCheck;

    [Header("基本属性")]
    public float walkSpeed;
    public float runSpeed;

    public float waitTime;
    public float waitCounter;

    [Header("状态")]
    public bool isWalk;
    public bool isRun;
    public bool isWait;
    [Space]
    public int facingDir = -1;

    protected Vector3 originScale;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();
    }

    private void Start()
    {
        originScale = transform.localScale;

        // 默认向左
        facingDir = originScale.x > 0 ? -1 : 1;

        isWalk = true;
        waitCounter = waitTime;
    }

    private void Update()
    {
        CheckFacingDirection();
    }

    private void CheckFacingDirection()
    {
        // 撞墙
        if (physicsCheck.isLeftWall && facingDir < 0 || physicsCheck.isRightWall && facingDir > 0)
        {
            // 等待后转身
            isWait = true;
            isWalk = false;
        }

        CountDownWaitTime();
    }

    private void CountDownWaitTime()
    {
        if (isWait)
        {
            waitCounter -= Time.deltaTime;
            if (waitCounter <= 0f)
            {
                waitCounter = waitTime;
                // 从等待状态切换回walk转台
                isWait = false;
                isWalk = true;
                // 转身
                facingDir = -facingDir;
                Flip();
            }
        }

    }

    private void FixedUpdate()
    {
        Move();
    }

    protected virtual void Move()
    {
        if (isRun)
        {
            SetVelocityX(runSpeed);
            animator.SetBool("isRun", true);
        }
        else if (isWalk)
        {
            SetVelocityX(walkSpeed);
            animator.SetBool("isWalk", true);
        }
        else if (isWait)
        {
            SetVelocityX(0);
            animator.SetBool("isWalk", false);
        }

        Flip();
    }

    protected void SetVelocityX(float speed)
    {
        rb.velocity = new Vector2(facingDir * speed, rb.velocity.y);
    }

    protected void Flip()
    {
        transform.localScale = new Vector3(facingDir * -1 * originScale.x, originScale.y, originScale.z);
    }
}
