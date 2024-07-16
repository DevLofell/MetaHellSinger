using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEarthQuakePattern : BossPattern
{
    public GameObject prefab;

    public Transform target;

    public List<Vector3> randomPos;

    public int spawningCount = 20;

    public float rangeLimit = 10f;

    public float fireTime = 0;
    public float fireDelay = 0.3f;

    public int nowFireIndex = 0;

    public float safeDistance = 5f; // 플레이어와의 최소 거리

    public override void Start()
    {
        base.Start();
        randomPos = randomPos ?? new List<Vector3>();

        foreach (var pos in randomPos)
        {
            GameObject go = GameObject.Instantiate(prefab);
            go.transform.position = pos;
        }
    }

    public override void Update()
    {
        base.Update();
        fireTime += Time.deltaTime;
        if (fireTime > fireDelay)
        {
            GameObject go = GameObject.Instantiate(prefab);
            go.transform.position = randomPos[nowFireIndex];

            if (nowFireIndex >= randomPos.Count - 1)
            {
                nowFireIndex = 0;
                isPatternOver = true;
                this.gameObject.SetActive(false);
            }
            else
            {
                nowFireIndex++;
            }
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        for (int i = 0; i < spawningCount; i++)
        {
            Vector3 randomPosition;
            do
            {
                float x = Random.Range(-rangeLimit, rangeLimit);
                float z = Random.Range(-rangeLimit, rangeLimit);
                randomPosition = target.position + new Vector3(x, 0, z);
            } while (Vector3.Distance(target.position, randomPosition) < safeDistance);

            randomPos.Add(randomPosition);
        }
    }

    public override void OnDisable()
    {
        base.OnDisable();
        randomPos.Clear();
    }
}
