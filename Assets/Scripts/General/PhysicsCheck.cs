using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    private CapsuleCollider2D capsuleCld;

    public bool isManual;

    [Header("检测参数")]
    public Vector2 bottomOffset;
    public Vector2 leftOffset;
    public Vector2 rightOffset;
    public float checkRadius;
    public LayerMask groundLayerMask;

    [Header("状态")]
    public bool isGround;
    public bool isLeftWall;
    public bool isRightWall;

    private void Awake()
    {
        capsuleCld = GetComponent<CapsuleCollider2D>();
    }

    private void Start()
    {
        if (!isManual)
        {
            leftOffset = new Vector2(capsuleCld.offset.x - capsuleCld.bounds.extents.x, capsuleCld.bounds.extents.y);

            rightOffset = new Vector2(capsuleCld.offset.x + capsuleCld.bounds.extents.x, capsuleCld.bounds.extents.y);
        }
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
        isLeftWall = Physics2D.OverlapCircle(new Vector2(transform.position.x + leftOffset.x, transform.position.y + leftOffset.y), checkRadius, groundLayerMask);

        isRightWall = Physics2D.OverlapCircle(new Vector2(transform.position.x + rightOffset.x, transform.position.y + rightOffset.y), checkRadius, groundLayerMask);
    }

    private void OnDrawGizmosSelected()
    {
        DisplayDetection();
    }

    private void DisplayDetection()
    {
        int facingDir = transform.localScale.x > 0 ? 1 : -1;

        Gizmos.DrawWireSphere(new Vector2(transform.position.x + bottomOffset.x * facingDir, transform.position.y + bottomOffset.y), checkRadius);

        Gizmos.DrawWireSphere(new Vector2(transform.position.x + leftOffset.x, transform.position.y + leftOffset.y), checkRadius);

        Gizmos.DrawWireSphere(new Vector2(transform.position.x + rightOffset.x, transform.position.y + rightOffset.y), checkRadius);
    }
}
