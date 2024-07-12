using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class EnemyController_GH : MonoBehaviour
{
    //���ʹ� Ȯ��
    public LayerMask enemyLayer;


    //��������
    public bool isDead = false;

    //�׷α� ����
    public bool isGroggy = false;
    float groggyDuration = 3.0f;
    float groggyTimer = 0;
    int groggyCount = 0;

    //�׷α�UI
    public GameObject groggyUI;

    //���Ͻ�ų
    private bool isStunned = false;
    private float stunDuration = 5f;
    private float stunTimer = 0.0f;
    //���� ���� ����
    public float stunDeadRadius = 5.0f;



    public Animator enemyAni;

    //�� ü��
    public float enemyCurrHP = 0;
    public float enemyMaxHP = 1000;

    //������ UI
    public GameObject damageUI;
    //�������� �ߴ� ��ġ
    public Transform damagePos;
    //������ UI�� ��Ʈ������
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
        //���� ü���� 10�� ���� �̰� �׷α⸦ �� ���� �Ȱɷ��� �� �׷α� ���·� �����.
        if ((enemyCurrHP / enemyMaxHP) <= 0.1f && groggyCount < 1)
        {
            GameObject groggy = Instantiate(groggyUI, GameObject.Find("Canvas").transform);
            Groggy();
        }


        //���� ���� ������ ��
        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            enemyAni.SetTrigger("OnStun");
            //���� ���� �����϶� ������
            if (isDead)
            {
                // �ֺ��� ������ Ž���ϰ�
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, stunDeadRadius, enemyLayer);
                foreach (Collider hitCollider in hitColliders)
                {
                    EnemyController_GH enemy = hitCollider.GetComponent<EnemyController_GH>();
                    if (enemy != null)
                    {
                        // �ֺ� ������ ���� �������� Ȯ���ϰ�
                        if (enemy.isStunned)
                        {
                            //�� ������ ü���� �� 0���� �����.
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
            //�� ������
        }
    }

    //������ �ޱ�
    public void TakeDamage(int damage)
    {
        enemyCurrHP -= damage;

        if (enemyCurrHP < 0)
        {
            enemyCurrHP = 0;
        }
        Debug.Log("���� ü�� : " + enemyCurrHP);
    }
    //������ UI
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

        print("�׷α��Դϴ�.");
        // ���ϻ��°� �ǰ�
        isGroggy = true;
        groggyTimer = groggyDuration;
        groggyCount++;
        // ���� �߾ӿ� E��� �۾��� ���

    }

}
