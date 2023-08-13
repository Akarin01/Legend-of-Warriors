using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInputControl inputControl;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private PlayerAnimation playerAnimation;

    public Vector2 inputDirection;

    [Header("基本参数")]
    public float runSpeed = 5f;
    public float walkSpeed = 2.5f;
    public float attackSpeed = 1f;
    public float jumpForce = 16f;
    public float hurtForce = 5f;

    private Vector3 originScale;


    [Header("状态")]
    public bool isHurt;
    public bool isDead;
    public bool isAttack;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputControl = new PlayerInputControl();
        physicsCheck = GetComponent<PhysicsCheck>();
        playerAnimation = GetComponent<PlayerAnimation>();

        inputControl.Gameplay.Jump.started += Jump;
        inputControl.Gameplay.Attack.started += PlayerAttack;
    }


    private void Start()
    {
        originScale = transform.localScale;
    }

    private void OnEnable()
    {
        EnableInputControl();
    }

    private void OnDisable()
    {
        DisableInputControl();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        GetInput();
    }

    // test
    // private void OnTriggerStay2D(Collider2D other)
    // {
    //     Debug.Log(other.name);
    // }

    /// <summary>
    /// 处理各状态下的人物移动
    /// </summary>
    private void Move()
    {
        // 受击状态
        if (isHurt)
            return;

        // 攻击状态
        if (isAttack)
        {
            int facingDir = transform.localScale.x > 0 ? 1 : -1;
            rb.velocity = new Vector2(facingDir * attackSpeed, rb.velocity.y);
        }
        // 行走状态
        else if (inputControl.Gameplay.Walk.IsPressed() && physicsCheck.IsGround)
        {
            SetVelocityX(walkSpeed);
        }
        // 其他状态
        else
        {
            SetVelocityX(runSpeed);
        }

        Flip();
    }

    private void Flip()
    {
        if (inputDirection.x == 0)
            return;

        int facingDir = inputDirection.x > 0 ? 1 : -1;
        SetFacingDirection(facingDir);

        // if (inputDirection.x == 0)
        //     return;

        // sr.flipX = inputDirection.x < 0;
    }

    private void SetFacingDirection(int facingDir)
    {
        transform.localScale = new Vector3(originScale.x * facingDir, originScale.y, originScale.z);
    }

    /// <summary>
    /// 死亡
    /// </summary>
    public void Die()
    {
        isDead = true;
        // 关闭Gameplay输入
        DisableGameplayInput();
    }

    public void GetHurt(Transform attacker)
    {
        isHurt = true;
        // 将速度归零
        rb.velocity = Vector2.zero;
        // 设置人物朝向
        int facingDir = attacker.position.x - transform.position.x > 0f ? 1 : -1;
        SetFacingDirection(facingDir);
        // 将角色推动
        Vector2 dir = new Vector2(transform.position.x - attacker.position.x, 0).normalized;
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
    }

    private void DisableGameplayInput()
    {
        inputControl.Gameplay.Disable();
    }

    private void SetVelocityX(float speed)
    {
        rb.velocity = new Vector2(inputDirection.x * speed, rb.velocity.y);
    }


    private void Jump(InputAction.CallbackContext context)
    {
        if (!physicsCheck.IsGround)
            return;

        rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
    }


    private void PlayerAttack(InputAction.CallbackContext context)
    {
        isAttack = true;
        playerAnimation.TriggerAttackAnimation();
    }

    private void GetInput()
    {
        inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();
    }

    private void EnableInputControl()
    {
        inputControl.Enable();
    }

    private void DisableInputControl()
    {
        inputControl.Disable();
    }

}
