using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    private Camera _camera;
    private ParticleSystem _particleSystem;

    public int weaponPower;
    public GameObject bulletEffect;
    
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        weaponPower = 450;
        _particleSystem = bulletEffect.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        // 마우스 왼쪽 버튼 클릭
        if (Input.GetMouseButtonDown(0))
        {
            var ray = new Ray(_camera.transform.position, _camera.transform.forward);
            var hitInfo = new RaycastHit();
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    var eFSM = hitInfo.transform.GetComponent<EnemyFsmJiwon>();
                    eFSM.HitEnemy(weaponPower);
                }
                else
                {
                    bulletEffect.transform.position = hitInfo.point;

                    bulletEffect.transform.forward = hitInfo.normal;

                    _particleSystem.Play();
                }
                
            }
        }
    }
}
