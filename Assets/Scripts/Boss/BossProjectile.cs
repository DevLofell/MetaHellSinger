using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public Transform target;
    public float speed = 3.0f;
    public Vector3 direction;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitUntil(()=>(target != null));
        direction = target.position - this.transform.position;
        direction.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        this.transform.Translate(direction * speed * Time.deltaTime);
    }
}
