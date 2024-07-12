using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class EnemyController_GH : MonoBehaviour
{
    //에너미 확인
    public LayerMask enemyLayer;


    //죽음상태
    public bool isDead = false;

    //그로기 상태
    public bool isGroggy = false;
    float groggyDuration = 3.0f;
    float groggyTimer = 0;
    int groggyCount = 0;

    //그로기UI
    public GameObject groggyUI;

    //스턴스킬
    private bool isStunned = false;
    private float stunDuration = 5f;
    private float stunTimer = 0.0f;
    //스턴 연계 죽음
    public float stunDeadRadius = 5.0f;



    public Animator enemyAni;

    //적 체력
    public float enemyCurrHP = 0;
    public float enemyMaxHP = 1000;

    //데미지 UI
    public GameObject damageUI;
    //데미지가 뜨는 위치
    public Transform damagePos;
    //데미지 UI의 렉트포지션
    public RectTransform rtDamageUI;
    private void Start()
    {
        enemyCurrHP = enemyMaxHP;
    }


    private void Update()
    {

        if (enemyCurrHP <= 0)
        {
            isDead = true;
            isGroggy = false;
        }
        //적의 체력이 10퍼 이하 이고 그로기를 한 번도 안걸렸을 때 그로기 상태로 만든다.
        if ((enemyCurrHP / enemyMaxHP) <= 0.1f && groggyCount < 1)
        {
            GameObject groggy = Instantiate(groggyUI, GameObject.Find("Canvas").transform);
            Groggy();
        }


        //적이 스턴 상태일 때
        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            enemyAni.SetTrigger("OnStun");
            //만약 스턴 상태일때 죽으면
            if (isDead)
            {
                // 주변의 적들을 탐색하고
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, stunDeadRadius, enemyLayer);
                foreach (Collider hitCollider in hitColliders)
                {
                    EnemyController_GH enemy = hitCollider.GetComponent<EnemyController_GH>();
                    if (enemy != null)
                    {
                        // 주변 적들이 스턴 상태인지 확인하고
                        if (enemy.isStunned)
                        {
                            //그 적들의 체력을 다 0으로 만든다.
                            enemy.enemyCurrHP = 0;

                        }
                    }
                }
            }


            if (stunTimer <= 0)
            {
                isStunned = false;
            }
        }
        else if (isGroggy)
        {
            groggyTimer -= Time.deltaTime;
            enemyAni.SetTrigger("OnStun");
            if (groggyTimer <= 0)
            {
                isGroggy = false;
            }
        }
        else
        {
            //적 움직임
        }
    }

    //데미지 받기
    public void TakeDamage(int damage)
    {
        enemyCurrHP -= damage;

        if (enemyCurrHP < 0)
        {
            enemyCurrHP = 0;
        }
        Debug.Log("남은 체력 : " + enemyCurrHP);
    }
    //데미지 UI
    public void OnDamageUI(int damageValue)
    {
        GameObject damage = Instantiate(damageUI, GameObject.Find("Canvas").transform);
        Text damageText = damage.GetComponent<Text>();
        damageText.text = Convert.ToString(damageValue);



        //Vector3 pos = Camera.main.WorldToScreenPoint(damagePos.position);
        DamageSystem ds = damage.GetComponent<DamageSystem>();
        ds.DamageMove(damagePos);
        Destroy(damage, 2);
    }
    public void OnGroggyUI(int damageValue)
    {
        GameObject damage = Instantiate(damageUI, GameObject.Find("Canvas").transform);
        Text damageText = damage.GetComponent<Text>();
        damageText.text = Convert.ToString(damageValue);



        //Vector3 pos = Camera.main.WorldToScreenPoint(damagePos.position);
        DamageSystem ds = damage.GetComponent<DamageSystem>();
        ds.DamageMove(damagePos);
        Destroy(damage, 2);
    }
    public void Stun()
    {
        isStunned = true;
        stunTimer = stunDuration;
    }

    public void Groggy()
    {

        print("그로기입니다.");
        // 스턴상태가 되고
        isGroggy = true;
        groggyTimer = groggyDuration;
        groggyCount++;
        // 적의 중앙에 E라는 글씨가 뜬다

    }

}
