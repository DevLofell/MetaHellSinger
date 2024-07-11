using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;


public class Player : MonoBehaviour
{
    //�÷��̾� ü��
    public float currHP = 0;
    public float maxHP = 1000;


    //�������� �̵�
    bool onOneShot = false;
    EnemyController_GH nearestEnemy = null;
    float speTime = 0;

    //�ذ� Ư������
    public LayerMask enemyLayer;
    public float stunRadius = 5.0f;

    #region �÷��̾� �⺻ ������
    //ī�޶� ȸ�� �ӵ� ��
    public float rotSpeed = 200f;

    //�÷��̾� ���콺 ȸ��
    float mx = 0;

    //�÷��̾� ���ǵ�
    public float moveSpeed = 7f;

    //�÷��̾� �̵� ����
    public Vector3 dir;

    CharacterController cc;
    #endregion

    #region ���� ����
    //����
    //�߷� ����
    public float gravity = -4f;
    //���� �ӷ� ����
    public float yVelocity = 0;
    //������ ����
    public float jumpPower = 1.7f;
    //�������� ����
    public bool isJumping = false;
    //���� ���� ��
    int nowJumpCount = 0;
    //�ִ� ���� ��
    int maxJumpCount = 2;

    #endregion

    //���Ÿ�����
    //���̾ ������
    public GameObject fireBallFactory;
    //���̾ ���� ��ġ
    public GameObject fireBallPos;
    //���̾ ����
    public Vector3 fireBallDir;



    //�ǰ� ��ƼŬ �ý���
    ParticleSystem ps;

    //�ִϸ�����
    public Animator anim;
    private PlayerAnimator playerAnimator;

    
    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        playerAnimator = GetComponentInChildren<PlayerAnimator>();
    }

    void Start()
    {
        cc = GetComponent<CharacterController>();
        currHP = maxHP;
        //ps = bulletEffect.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (onOneShot)
        {

            speTime += Time.deltaTime;
            //���� ���� �Ÿ�
            float distance;

            distance = Vector3.Distance(transform.position, nearestEnemy.transform.position);
            if (distance > 2.5f)
            {
                Vector3 spedir = nearestEnemy.transform.position - transform.position;
                spedir.Normalize();
                transform.Translate(spedir * 100 * Time.deltaTime, Space.World);

            }
            else
            {

            }

            if (speTime > 1)
            {
                onOneShot = false;
                speTime = 0;
                nearestEnemy = null;
            }



        }
        else
        {
            PlayerMove();
            PlayerRotate();
            PlayerFire();

        }
    }

    void PlayerRotate()
    {
        
        float mouseX = Input.GetAxis("Mouse X");

        mx += mouseX * rotSpeed * Time.deltaTime;


        transform.eulerAngles = new Vector3(0, mx, 0);
    }

    void PlayerMove()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");


        dir = new Vector3(h, 0, v);

        float scalar = dir.magnitude;


        if (scalar > 1)
        {
            scalar = 1f;
        }

        dir = Camera.main.transform.TransformDirection(dir);
        dir.y = 0;
        dir = dir.normalized * scalar;

        //���� ���?
        //�����̽��� ������ ����

        if (!cc.isGrounded)
        {
            //ĳ���� ���� �ӵ��� �߷� ���� ����
            yVelocity += gravity * Time.deltaTime;

        }
        else
        {
            yVelocity = 0;
        }




        if (cc.collisionFlags == CollisionFlags.Below && isJumping)
        {

            isJumping = false;
            yVelocity = 0;
            nowJumpCount = 0;
        }



        if (Input.GetButtonDown("Jump") && nowJumpCount < maxJumpCount)
        {
            yVelocity = jumpPower;
            isJumping = true;
            nowJumpCount++;
        }


        dir.y = yVelocity;


        cc.Move(dir * moveSpeed * Time.deltaTime);
        //transform.position += dir * moveSpeed * Time.deltaTime;


        //�ִϸ��̼�
        //���� �ִϸ��̼� ����
        playerAnimator.OnMovement(h, v);

    }

    public void Fire()
    {
        //���� ���°� Į�� ��
        if (playerAnimator.animator.GetInteger("weaponState") == 0)
        {
            ////Į�� �ִϸ��̼� ����
            playerAnimator.OnSwordAttack();

        }
        if (playerAnimator.animator.GetInteger("weaponState") == 1)
        {
            #region �÷��̾� ���Ÿ�
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            RaycastHit hitInfo = new RaycastHit();

            //���̾�� �����Ѵ�
            GameObject fireBall = Instantiate(fireBallFactory);
            //���̾�� ��ġ�� ������ġ�� �����?
            fireBall.transform.position = fireBallPos.transform.position;
            //���̸� ���µ�...
            //���𰡿� �ε������� �ε��� ������
            if (Physics.Raycast(ray, out hitInfo))
            {

                //���̾�� ������ ȭ�� �߾�(����)���� �Ѵ�.
                fireBallDir = hitInfo.point - fireBall.transform.position;
                //������ �̵��Ѵ�.
                fireBall.transform.forward = fireBallDir.normalized;

            }
            //�ε������� ������ ī�޶� �ٶ󺸴� ��������
            else
            {
                //���̾�� ���� ���������� �ٲ۴�.
                fireBall.transform.forward = Camera.main.transform.forward;

            }
            //2�� �ڿ� ���̾�� �ı��Ѵ�.
            Destroy(fireBall, 2);
            #endregion

        }
    }

    public void PlayerFire()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            OneShot();

        }

        //���� ���°� Į�� ��
        if (playerAnimator.animator.GetInteger("weaponState") == 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                ////Į�� �ִϸ��̼� ����
                playerAnimator.OnSwordAttack();

                onOneShot = false;

            }
            if (Input.GetButtonDown("Fire2"))
            {
                playerAnimator.OnSpeSwordState();
            }
        }

        if (playerAnimator.animator.GetInteger("weaponState") == 1)
        {
            if (Input.GetButtonDown("Fire1"))
            {

                #region �÷��̾� ���Ÿ�
                Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

                RaycastHit hitInfo = new RaycastHit();

                //���̾�� �����Ѵ�
                GameObject fireBall = Instantiate(fireBallFactory);
                //���̾�� ��ġ�� ������ġ�� �����.
                fireBall.transform.position = fireBallPos.transform.position;
                //���̸� ���µ�...
                //���𰡿� �ε������� �ε��� ������
                if (Physics.Raycast(ray, out hitInfo))
                {
                    //���̾�� ������ ȭ�� �߾�(����)���� �Ѵ�.
                    fireBallDir = hitInfo.point - fireBall.transform.position;
                    //������ �̵��Ѵ�.
                    fireBall.transform.forward = fireBallDir.normalized;
                }
                //�ε������� ������ ī�޶� �ٶ󺸴� ��������
                else
                {
                    //���̾�� ���� ���������� �ٲ۴�.
                    fireBall.transform.forward = Camera.main.transform.forward;
                }
                //2�� �ڿ� ���̾�� �ı��Ѵ�.
                Destroy(fireBall, 2);
                #endregion
            }

            if (Input.GetButtonDown("Fire2"))
            {
                StunSkill();
            }
        }

        //���� ���°� Į�� ��
        if (playerAnimator.animator.GetInteger("weaponState") == 2)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                ////Į�� �ִϸ��̼� ����
                playerAnimator.OnSpeSwordAttack();
            }
        }
        //1���� ������
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //�ִϸ��̼� ���¸� �������?�ٲٱ�
            playerAnimator.OnSwordState();
        }

        //2���� ������
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //�ִϸ��̼� ���¸� ���Ÿ��� �ٲٱ�
            playerAnimator.OnFireState();


        }
    }

    void StunSkill()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, stunRadius, enemyLayer);
        foreach (Collider hitCollider in hitColliders)
        {
            EnemyController_GH enemy = hitCollider.GetComponent<EnemyController_GH>();
            if (enemy != null)
            {
                Vector3 viewportPoint = Camera.main.WorldToViewportPoint(enemy.transform.position);
                if (viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1 && viewportPoint.z > 0)
                {
                    print("��ų�ߵ�");
                    enemy.Stun();
                }

            }
        }
    }
    void OneShot()
    {
        //�������� ���̸� ���� ���������ȿ� �ִ� ������ ���� ã�´�
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, stunRadius, enemyLayer);

        float nearestDistance = float.MaxValue;

        //ã�� ������ �迭�� �����Ѵ�
        foreach (Collider hitCollider in hitColliders)
        {
            EnemyController_GH enemy = hitCollider.GetComponent<EnemyController_GH>();
            if (enemy != null)
            {

                //����(1. ȭ�鳻�� �־�� �Ѵ� 2. ���� ����� ��)
                Vector3 viewportPoint = Camera.main.WorldToViewportPoint(enemy.transform.position);
                if (viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1 && viewportPoint.z > 0)
                {
                    if (enemy.isGroggy)
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
        if (nearestEnemy != null)
        {
            //���� ������ ������ �̵��Ѵ�.
            onOneShot = true;

            playerAnimator.GroggyAttack();

            //���� ü���� 0���� �����.
            nearestEnemy.enemyCurrHP = 0;
        }
    }
}
