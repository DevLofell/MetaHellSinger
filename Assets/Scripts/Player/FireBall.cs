using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    //파이어볼 스피드
    public float fireBallSpeed = 20f;
    //파이어볼 피격 이펙트 오브젝트
    public GameObject fireBallEffectFactory;
    //파이어볼 데미지
    public int fireBallDamage = 120;

    public Vector3 dir;
   
    void Update()
    {
        ////앞으로 이동한다.
        transform.position += transform.forward * fireBallSpeed * Time.deltaTime;
    }
    //파이어볼에 다른 물체가 감지되면
    private void OnTriggerEnter(Collider other)
    {
        //파이어볼 폭발 효과를 생성한다.
        GameObject fireBallEffect = Instantiate(fireBallEffectFactory);
        //파이어볼효과의 위치는 파이어볼의 위치로한다.
        fireBallEffect.transform.position = transform.position;
        //파이어볼을 파괴한다.
        Destroy(gameObject);
        //파이어볼 이펙트가 3초뒤 사라진다.
        Destroy(fireBallEffect,3);

        //만약 맞은게 적이라면
        if (other.CompareTag("Enemy"))
        {
            //데미지를 준다
            other.GetComponent<EnemyFsmJiwon>().HitEnemy(fireBallDamage);
        }
    }
   

}
