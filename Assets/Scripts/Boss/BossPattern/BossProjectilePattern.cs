using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectilePattern : BossPattern
{
    public GameObject Projectile;
    public Transform target;

    public int nowPosIndex = 0;
    public List<Transform> firePosList;

    public float nowFireTime = 0f; 
    public float nowPatternTime = 0f;

    public float firedelay = 0.3f;
    public float parrernDelay = 1f;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        nowFireTime += Time.deltaTime;
        if(nowFireTime > firedelay)
        {
            GameObject go = GameObject.Instantiate(Projectile);
            go.transform.position = firePosList[nowPosIndex].position;
            go.GetComponent<BossProjectile>().target = this.target;
            nowFireTime = 0f;

            if (nowPosIndex >= firePosList.Count - 1)
            {
                nowPosIndex = 0;
                isPatternOver = true;
                this.gameObject.SetActive(false);   
            }
            else
            {
                nowPosIndex++;
            }
            

        }
    }
}
