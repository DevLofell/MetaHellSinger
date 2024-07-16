using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyRanged : EnemyFsmJiwon
{
    // Start is called before the first frame update
    public override void Start()
    {
        maxHp = 2000;
        attackDelay = 5f;
        findDistance = 30f;
        moveDistance = 50f;
        attackDistance = 20f;
        attackPower = 10;
        groggyHp = maxHp / 10;
        hp = maxHp;
        mState = EnemyState.Idle;
        stunTimer = stunTime;
        base.Start();
    }

    protected override void Attack()
    {
        transform.LookAt(_player);
        base.Attack();
    }

}
