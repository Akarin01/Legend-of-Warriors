
using System.Diagnostics;
using UnityEngine;

public class BoarPatrolState : BaseState
{
    public BoarPatrolState(Enemy enemy) : base(enemy)
    {

    }

    public override void OnEnter()
    {
        enemy.currentSpeed = enemy.walkSpeed;
    }
    public override void LogicUpdate()
    {
        // 当发现玩家时
        if (enemy.FindPlayer())
        {
            // 切换到追击状态
            enemy.SwitchState(EnemyState.chase);
        }
        // 撞墙或走到悬崖旁时
        if (enemy.physicsCheck.isWall || !enemy.physicsCheck.isGround)
        {
            // 等待后转身
            enemy.isWait = true;
            enemy.animator.SetBool("isWait", true);
        }
    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {

    }

}
