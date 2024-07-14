using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    //���̾ ���ǵ�
    public float fireBallSpeed = 20f;
    //���̾ �ǰ� ����Ʈ ������Ʈ
    public GameObject fireBallEffectFactory;
    //���̾ ������
    public int fireBallDamage = 120;

    public Vector3 dir;
   
    void Update()
    {
        ////������ �̵��Ѵ�.
        transform.position += transform.forward * fireBallSpeed * Time.deltaTime;
    }
    //���̾�� �ٸ� ��ü�� �����Ǹ�
    private void OnTriggerEnter(Collider other)
    {
        //���̾ ���� ȿ���� �����Ѵ�.
        GameObject fireBallEffect = Instantiate(fireBallEffectFactory);
        //���̾ȿ���� ��ġ�� ���̾�� ��ġ���Ѵ�.
        fireBallEffect.transform.position = transform.position;
        //���̾�� �ı��Ѵ�.
        Destroy(gameObject);
        //���̾ ����Ʈ�� 3�ʵ� �������.
        Destroy(fireBallEffect,3);

        //���� ������ ���̶��
        if (other.CompareTag("Enemy"))
        {
            //�������� �ش�
            other.GetComponent<EnemyFsmJiwon>().HitEnemy(fireBallDamage);
        }
    }
   

}
