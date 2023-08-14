using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    private CapsuleCollider2D capsuleCld;

    [Header("检测参数")]
    public Vector2 bottomOffset;
    public Vector2 facingOffset;
    public float checkRadius;
    public LayerMask groundLayerMask;

    [Header("状态")]
    public bool isGround;
    public bool isWall;

    private void Awake()
    {
        capsuleCld = GetComponent<CapsuleCollider2D>();
    }

    private void Start()
    {

    }

    void Update()
    {
        CheckGround();
        CheckWall();
    }

    private void CheckGround()
    {
        int facingDir = transform.localScale.x > 0 ? 1 : -1;

        isGround = Physics2D.OverlapCircle(new Vector2(transform.position.x + bottomOffset.x * facingDir, transform.position.y + bottomOffset.y), checkRadius, groundLayerMask);
    }

    private void CheckWall()
    {
        int facingDir = transform.localScale.x > 0 ? 1 : -1;

        isWall = Physics2D.OverlapCircle(new Vector2(transform.position.x + facingOffset.x * facingDir, transform.position.y + facingOffset.y), checkRadius, groundLayerMask);
    }

    private void OnDrawGizmosSelected()
    {
        DisplayDetection();
    }

    private void DisplayDetection()
    {
        int facingDir = transform.localScale.x > 0 ? 1 : -1;

        Gizmos.DrawWireSphere(new Vector2(transform.position.x + bottomOffset.x * facingDir, transform.position.y + bottomOffset.y), checkRadius);

        Gizmos.DrawWireSphere(new Vector2(transform.position.x + facingOffset.x * facingDir, transform.position.y + facingOffset.y), checkRadius);
    }
}
