
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyFactory;

    private int _enemyLimit;
    private int _index;
    private float _currentTime;
    private const float CreateTime = 3f;
    private GameObject[] _enemy;

    private void Start()
    {
        _enemyLimit = 5;
        _enemy = new GameObject[_enemyLimit];

        for (var i = 0; i < _enemyLimit; i++)
        {
            _enemy[i] = Instantiate(enemyFactory);
            _enemy[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_index >= _enemyLimit) return;
        
        _currentTime += Time.deltaTime;
        
        if (!(_currentTime > CreateTime)) return;
        _currentTime = 0;
        _enemy[_index].transform.Rotate(0, Random.Range(0.0f, 360.0f), 0);
        _enemy[_index].transform.Translate(Vector3.forward * Random.Range(1.0f, 5.0f));
        _enemy[_index++].SetActive(true);
    }

}
