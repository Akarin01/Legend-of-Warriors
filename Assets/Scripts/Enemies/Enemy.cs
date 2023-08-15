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
    public float chaseSpeed;
    public float hurtForce;

    public float waitTime;
    public float waitCounter;

    public float recoverFromHurtTime;
    private WaitForSeconds recoverFromHurt;

    [Header("状态")]
    public bool isChase;
    public bool isWait;
    public bool isHurt;
    public bool isDead;
    [Space]
    public int facingDir;

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

        waitCounter = waitTime;

        recoverFromHurt = new WaitForSeconds(recoverFromHurtTime);
    }

    private void Update()
    {
        ChangeFacingDirection();
    }

    private void ChangeFacingDirection()
    {
        // 撞墙
        if (physicsCheck.isWall)
        {
            // 等待后转身
            isWait = true;
            animator.SetBool("isWait", true);
        }

        CountDownWaitTime();
    }

    /// <summary>
    /// wait时间计数器
    /// </summary>
    private void CountDownWaitTime()
    {
        if (isWait)
        {
            waitCounter -= Time.deltaTime;
            if (waitCounter <= 0f)
            {
                waitCounter = waitTime;
                // 从等待状态切换回walk状态
                isWait = false;
                animator.SetBool("isWait", false);
                // 转身
                facingDir = -facingDir;
                Flip();
            }

            // 在wait状态受伤
            if (isHurt)
            {
                waitCounter = waitTime;
                // 从等待状态切换回walk状态
                isWait = false;
                animator.SetBool("isWait", false);
            }
        }

    }

    private void FixedUpdate()
    {
        Move();
    }

    protected virtual void Move()
    {
        if (isHurt || isDead)
            return;
        if (isChase)
        {
            SetVelocityX(chaseSpeed);
        }
        else if (isWait)
        {
            SetVelocityX(0);
        }
        else
        {
            SetVelocityX(walkSpeed);
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

    public void TakeDamage(Transform attacker)
    {
        isHurt = true;
        animator.SetTrigger("hurt");

        // 计算击退方向
        Vector2 dir = (transform.position - attacker.position).normalized;

        // 重置速度
        rb.velocity = Vector2.zero;

        // 设置敌人方向
        facingDir = dir.x < 0 ? 1 : -1;
        Flip();

        // 击退
        StartCoroutine(OnHurt(dir));

    }

    private IEnumerator OnHurt(Vector2 forceDir)
    {
        rb.AddForce(hurtForce * forceDir, ForceMode2D.Impulse);

        yield return recoverFromHurt;
        isHurt = false;
    }

    public void OnDie()
    {
        gameObject.layer = 9;
        isDead = true;
        animator.SetBool("isDead", true);
    }

    public void DestroyAfterDeath()
    {
        Destroy(gameObject);
    }
}
