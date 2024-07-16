using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    public GameObject bulletFactory;
    public GameObject firePosition;
    public GameObject targetPosition;

    private int _bulletLength;
    private GameObject[] _enemyBulletArray;
    // Start is called before the first frame update
    void Start()
    {
        _bulletLength = 3;
        _enemyBulletArray = new GameObject[_bulletLength];
        targetPosition = GameObject.FindWithTag("Player");
        
        for (var i = 0; i < _bulletLength; i++)
        {
            _enemyBulletArray[i] = Instantiate(bulletFactory);
            _enemyBulletArray[i].SetActive(false);
        }
    }

    public void FireToPlayer()
    {
        
        for (var i = 0; i < _bulletLength; i++)
        {
            if (_enemyBulletArray[i].activeSelf) continue;
            var enemyBullet = _enemyBulletArray[i].GetComponent<EnemyBullet>();
            _enemyBulletArray[i].transform.position = firePosition.transform.position;
            enemyBullet.FireBullet(targetPosition.transform.position - firePosition.transform.position);
            break;
        }
    }
}
