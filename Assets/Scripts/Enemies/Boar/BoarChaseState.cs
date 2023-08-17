using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarChaseState : BaseState
{
    public BoarChaseState(Enemy enemy) : base(enemy)
    {

    }

    public override void OnEnter()
    {
        enemy.isChase = true;
        enemy.currentSpeed = enemy.chaseSpeed;
        enemy.animator.SetBool("isChase", true);
    }

    public override void LogicUpdate()
    {
        // 撞墙或走到悬崖旁时
        if (enemy.physicsCheck.isWall || !enemy.physicsCheck.isGround)
        {
            // 立刻转身
            enemy.facingDir = new Vector2(-enemy.facingDir.x, 0);
            enemy.Flip();
        }

        // 当不处于追击状态时
        if (!enemy.isChase)
        {
            enemy.SwitchState(EnemyState.patrol);
        }
    }
    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {
        enemy.animator.SetBool("isChase", false);
    }

}
