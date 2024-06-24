using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //ī�޶� ȸ�� �ӵ� ��
    public float rotSpeed = 200f;

    //�÷��̾� ���ǵ�
    public float moveSpeed = 7f;

    //�÷��̾� �̵� ����
    public Vector3 dir;

    //����
    //�߷� ����
    public float gravity = -10f;
    //���� �ӷ� ����
    public float yVelocity = 0;
    //������ ����
    public float jumpPower = 10f;
    //�������� ����
    public bool isJumping = false;

    int nowJumpCount = 0;
    int maxJumpCount = 2;

    CharacterController cc;

    //�÷��̾� ���콺 ȸ��
    float mx = 0;


    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
        PlayerRotate();
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

        //ĳ���� ���� �ӵ��� �߷� ���� ����
        yVelocity += gravity * Time.deltaTime;




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
    }



}
