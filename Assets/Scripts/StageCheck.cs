using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StageCheck : MonoBehaviour
{
    public static StageCheck instance;

    public List<GameObject> listStage01 = new List<GameObject>();
    public List<GameObject> listStage02 = new List<GameObject>();
    public List<GameObject> listStage03 = new List<GameObject>();

    public Transform stage01door;

    public Transform stage02doorL;
    public Transform stage02doorR;
    public GameObject stage02Wall;



    float doorYrotateValue = 0;
    float door2YrotateValue = 0;
    float door3YrotateValue = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //스테이지01 문열림
        if (listStage01.Count == 0 && doorYrotateValue >= -90f)
        {
            doorYrotateValue -= Time.deltaTime * 10f;

            stage01door.localEulerAngles = new Vector3(0, doorYrotateValue, 0);
        }

        //스테이지02 문열림
        if (listStage02.Count == 0)
        {
            if (door2YrotateValue >= -90f)
            {
                door2YrotateValue -= Time.deltaTime * 10f;

                stage02doorL.localEulerAngles = new Vector3(0, door2YrotateValue, 0);
            }
            if (door3YrotateValue <= 90f)
            {
                door3YrotateValue += Time.deltaTime * 10f;

                stage02doorR.localEulerAngles = new Vector3(0, door3YrotateValue, 0);
            }
            stage02Wall.SetActive(false);
        }
       
    }

    public void EnemyDead(GameObject enemy)
    {
        for (int i = 0; i < listStage01.Count; i++)
        {
            if (enemy == listStage01[i])
            {
                listStage01.RemoveAt(i);
            }
        }

        for (int i = 0; i < listStage02.Count; i++)
        {
            if (enemy == listStage02[i])
            {
                listStage02.RemoveAt(i);
            }
        }
    }
}
