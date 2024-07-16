using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : EnemyFsmJiwon
{
    // Start is called before the first frame update
    void Start()
    {
        maxHp = 1000;
        attackDelay = 2f;
        findDistance = 10f;
        moveDistance = 20f;
        attackDistance = 5f;
        attackPower = 5;
        groggyHp = maxHp / 10;
        hp = maxHp;
        mState = EnemyState.Idle;
        stunTimer = stunTime;
        base.Start();
    }

}
