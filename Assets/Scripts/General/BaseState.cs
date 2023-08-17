using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    protected Enemy enemy;

    public BaseState(Enemy enemy)
    {
        this.enemy = enemy;
    }
    public abstract void OnEnter();
    public abstract void LogicUpdate();
    public abstract void PhysicsUpdate();
    public abstract void OnExit();
}
