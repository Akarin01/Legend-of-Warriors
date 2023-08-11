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

    public Vector2 inputDirection;

    [Header("基本参数")]
    public float runSpeed = 5f;
    public float walkSpeed = 2.5f;
    public float jumpForce = 16f;

    private Vector3 originScale;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputControl = new PlayerInputControl();
        physicsCheck = GetComponent<PhysicsCheck>();

        inputControl.Gameplay.Jump.started += Jump;
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

    private void Move()
    {
        // 按住Walk键进入Walk状态
        if (inputControl.Gameplay.Walk.IsPressed())
        {
            SetVelocityX(walkSpeed);
        }
        else
        {
            SetVelocityX(runSpeed);
        }

        Flip();
    }

    private void SetVelocityX(float speed)
    {
        rb.velocity = new Vector2(inputDirection.x * speed, rb.velocity.y);
    }

    private void Flip()
    {
        if (inputDirection.x == 0)
            return;

        int facingDir = inputDirection.x > 0 ? 1 : -1;
        transform.localScale = new Vector3(originScale.x * facingDir, originScale.y, originScale.z);

        // if (inputDirection.x == 0)
        //     return;

        // sr.flipX = inputDirection.x < 0;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (!physicsCheck.IsGround)
            return;

        rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
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
