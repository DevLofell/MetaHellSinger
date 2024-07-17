using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private const float Speed = 1f;
    private const int Power = 5;
    
    private Vector3 _dir;
    // Update is called once per frame
    void Update()
    {
        transform.position += Speed * Time.deltaTime * _dir;
    }

    public void FireBullet(Vector3 playerDir)
    {
        _dir = playerDir;
        gameObject.SetActive(true);
        StartCoroutine(Destroy());
    }
    
    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
    }

    //√—æÀ¿Ã √Êµπ
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Player"))
        {
            other.gameObject.GetComponent<Player>().UpdateHP(-Power);
            gameObject.SetActive(false);
        }
    }
}
