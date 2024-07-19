using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    //카메라 쉐이크
    public CameraShake cameraShake;

    //파이어볼 스피드
    public float fireBallSpeed = 20f;
    //파이어볼 피격 이펙트 오브젝트
    public GameObject fireBallEffectFactory;
    //파이어볼 데미지
    public int fireBallDamage = 120;

    public Vector3 dir;

    private void Start()
    {
        if(cameraShake == null)
        {
            cameraShake = Camera.main.GetComponent<CameraShake>();
        }

        //GetComponent<Rigidbody>().velocity = transform.forward * fireBallSpeed;
    }
    void Update()
    {
        ////앞으로 이동한다.
        transform.position += transform.forward * fireBallSpeed * Time.deltaTime;
    }



    //파이어볼에 다른 물체가 감지되면
    private void OnTriggerEnter(Collider other)
    {
        print("카메라 흔들");
        //카메라 쉐이크
        cameraShake.PlayShake(0.3f, 0.1f);

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
