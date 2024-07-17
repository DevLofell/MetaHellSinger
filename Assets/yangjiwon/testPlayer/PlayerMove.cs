using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 7f;
    public float jumpPower = 10f;
    public bool isJumping = false;
    public float yVelocity = 0;
    public int hp = 100;

    private float gravity = -20f;

    private CharacterController _characterController;
    
    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            StunSkill();
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            OneShot();
        }
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        var dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        dir = Camera.main.transform.TransformDirection(dir);

        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;


        if (_characterController.collisionFlags == CollisionFlags.Below)
        {
            if (isJumping)
            {
                isJumping = false;
                yVelocity = 0;
            }
        }

        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            yVelocity = jumpPower;
            isJumping = true;
        }


        if (Input.GetButtonDown("Jump"))
        {
            yVelocity = jumpPower;
        }

        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;
        
        _characterController.Move(dir * (moveSpeed * Time.deltaTime));
    }

    public void UpdateHP(int damage)
    {
        hp += damage;
        print("Player hp : " + hp);
        
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    void StunSkill()
    {
        print("StunSkill 호출");
        LayerMask enemyLayer = 1 << LayerMask.NameToLayer("Enemy");
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5, enemyLayer);
        foreach (Collider hitCollider in hitColliders)
        {
            EnemyFsmJiwon enemy = hitCollider.GetComponent<EnemyFsmJiwon>();
            
            if (enemy != null)
            {
                Vector3 viewportPoint = Camera.main.WorldToViewportPoint(enemy.transform.position);
                if (viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1 && viewportPoint.z > 0)
                {
                    print("스턴발생");
                    enemy.OnStunChanged();
                }

            }
        }
    }
    
    void OneShot()
    {
        print("oneShot 호출");
        LayerMask layerMask = LayerMask.GetMask("Enemy");
        //�������� ���̸� ���� ���������ȿ� �ִ� ������ ���� ã�´�
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 30f, layerMask);
        EnemyFsmJiwon nearestEnemy = null;
        float nearestDistance = float.MaxValue;

        //ã�� ������ �迭�� �����Ѵ�
        foreach (Collider hitCollider in hitColliders)
        {
            EnemyFsmJiwon enemy = hitCollider.GetComponent<EnemyFsmJiwon>();
            if (enemy)
            {
                //����(1. ȭ�鳻�� �־�� �Ѵ� 2. ���� ����� ��)
                Vector3 viewportPoint = Camera.main.WorldToViewportPoint(enemy.transform.position);
                if (viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1 && viewportPoint.z > 0)
                {
                    if (enemy.mState == EnemyState.Groggy)
                    {
                        float distance = Vector3.Distance(transform.position, enemy.transform.position);
                        if (distance < nearestDistance)
                        {
                            nearestDistance = distance;
                            nearestEnemy = enemy;
                        }

                    }
                }
            }
        }
        print(nearestEnemy + " / " + nearestDistance);
        if (nearestEnemy)
        {
            nearestEnemy.Die();
        }
    }
}
