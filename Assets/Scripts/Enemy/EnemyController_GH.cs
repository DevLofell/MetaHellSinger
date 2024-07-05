using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController_GH : MonoBehaviour
{
    public int enemyCurrHP = 0;
    public int enemyMaxHP = 1000;

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
    //데미지 받기
    public void TakeDamage(int damage)
    {
        enemyCurrHP -= damage;

        if(enemyCurrHP < 0)
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
}
