using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPattern : MonoBehaviour
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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
            }
            else
            {
                nowPosIndex++;
            }
            

        }
    }
}
