using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
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
    //���̾ ���ǵ�
    public float fireBallSpeed = 5.0f;
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

        //ps = bulletEffect.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
        PlayerRotate();
        PlayerFire();
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

        //���� ���
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
    }

    public void PlayerFire()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            

        }
        //1���� ������
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //�ִϸ��̼� ���¸� ������� �ٲٱ�
            playerAnimator.OnSwordState();
        }

        //2���� ������
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //�ִϸ��̼� ���¸� ���Ÿ��� �ٲٱ�
            playerAnimator.OnFireState();
        }
    }

}
