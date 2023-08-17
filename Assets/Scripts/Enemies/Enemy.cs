using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Rigidbody2D rb;
    [HideInInspector] public Animator animator;
    [HideInInspector] public PhysicsCheck physicsCheck;

    [Header("基本属性")]
    public float walkSpeed;
    public float chaseSpeed;
    public float currentSpeed;
    public float hurtForce;

    [Header("倒计时")]
    public float waitTime;
    public float waitCounter;
    public float lostTime;
    public float lostCounter;

    public float recoverFromHurtTime;
    private WaitForSeconds recoverFromHurt;

    [Header("状态")]
    public bool isWait;
    public bool isChase;
    public bool isHurt;
    public bool isDead;

    [Header("检测")]
    public Vector2 centerOffset;
    public Vector2 size;
    public float distance;
    public LayerMask playerLayerMask;


    [Space]
    public Vector2 facingDir;

    protected BaseState currentState;
    protected BaseState patrolState;
    protected BaseState chaseState;

    protected Vector3 originScale;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();
    }

    private void Start()
    {
        // 初始化默认状态
        currentState = patrolState;
        currentState.OnEnter();

        originScale = transform.localScale;

        // 默认向左
        facingDir = originScale.x > 0 ? new Vector2(-1, 0) : new Vector2(1, 0);

        waitCounter = waitTime;
        lostCounter = lostTime;

        recoverFromHurt = new WaitForSeconds(recoverFromHurtTime);
    }

    private void Update()
    {
        currentState.LogicUpdate();
        CountDownTime();
    }


    /// <summary>
    /// 时间计数器
    /// </summary>
    private void CountDownTime()
    {
        // wait时间计时
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

        //chase时间计时
        if (!FindPlayer() && isChase)
        {
            lostCounter -= Time.deltaTime;
            if (lostCounter <= 0f)
            {
                isChase = false;
                lostCounter = lostTime;
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
        if (isWait)
        {
            SetVelocityX(0);
        }
        else
        {
            SetVelocityX(currentSpeed);
        }

        Flip();
    }


    public bool FindPlayer()
    {
        return Physics2D.BoxCast(transform.position + (Vector3)centerOffset, size, 0, facingDir, distance, playerLayerMask);
    }

    public void SwitchState(EnemyState newState)
    {
        currentState.OnExit();
        currentState = newState switch
        {
            EnemyState.patrol => patrolState,
            EnemyState.chase => chaseState,
            _ => null,
        };
        currentState.OnEnter();
    }

    protected void SetVelocityX(float speed)
    {
        rb.velocity = new Vector2(facingDir.x * speed, rb.velocity.y);
    }

    public void Flip()
    {
        transform.localScale = new Vector3(facingDir.x * -1 * originScale.x, originScale.y, originScale.z);
    }

    public void TakeDamage(Transform attacker)
    {
        isHurt = true;
        animator.SetTrigger("hurt");

        // 计算击退方向
        Vector2 dir = (transform.position - attacker.position).normalized;

        // 重置速度
        rb.velocity = new Vector2(0, rb.velocity.y);

        // 设置敌人方向
        facingDir.x = dir.x < 0 ? 1 : -1;
        Flip();

        // 击退
        StartCoroutine(OnHurt(dir));

    }

    #region 事件调用方法

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
    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position + (Vector3)centerOffset, size);
        Gizmos.DrawWireCube(transform.position + (Vector3)centerOffset + new Vector3(distance * facingDir.x, 0, 0), size);
    }
}
