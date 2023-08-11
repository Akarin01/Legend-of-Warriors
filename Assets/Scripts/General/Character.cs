using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("基本属性")]
    public float maxHealth;
    public float currentHealth;

    [Header("受伤无敌")]
    public float invulnerableTime;
    private float invulnerableCount;
    public bool isInvulnerable;

    public UnityEvent<Transform> onHurted;
    public UnityEvent onDead;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        // 无敌时间倒计时
        if (isInvulnerable)
        {
            invulnerableCount -= Time.deltaTime;

            // 无敌时间结束
            if (invulnerableCount <= 0)
            {
                isInvulnerable = false;
            }
        }
    }

    public void TakeDamage(Attack attaker)
    {
        if (isInvulnerable) return;

        // 受伤但不死亡
        if (currentHealth - attaker.damage > 0)
        {
            currentHealth -= attaker.damage;
            TiggerInvulnerable();
            onHurted?.Invoke(attaker.transform);
        }
        // 死亡
        else
        {
            currentHealth = 0;
            onDead?.Invoke();
        }

    }

    /// <summary>
    /// 受伤后触发无敌时间
    /// </summary>
    private void TiggerInvulnerable()
    {
        isInvulnerable = true;
        invulnerableCount = invulnerableTime;
    }
}
