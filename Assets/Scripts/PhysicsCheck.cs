using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    public Vector2 bottomOffset;
    public float checkRadius;
    public LayerMask groundLayerMask;

    private bool isGround;

    public bool IsGround => isGround;
    void Update()
    {
        int facingDir = transform.localScale.x > 0 ? 1 : -1;

        isGround = Physics2D.OverlapCircle(new Vector2(transform.position.x + bottomOffset.x * facingDir, transform.position.y + bottomOffset.y), checkRadius, groundLayerMask);
    }

    private void OnDrawGizmosSelected()
    {
        int facingDir = transform.localScale.x > 0 ? 1 : -1;

        Gizmos.DrawWireSphere(new Vector2(transform.position.x + bottomOffset.x * facingDir, transform.position.y + bottomOffset.y), checkRadius);
    }
}
