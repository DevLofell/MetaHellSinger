using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StageCheck : MonoBehaviour
{
    public static StageCheck instance;

    public List<GameObject> listStage01 = new List<GameObject>();
    public List<GameObject> listStage02_1 = new List<GameObject>();
    public List<GameObject> listStage02_2 = new List<GameObject>();

    public List<GameObject> listStage03 = new List<GameObject>();

    public Transform stage01door;

    public Transform stage02doorL;
    public Transform stage02doorR;
    public GameObject stage02Wall;

    public AudioSource stage01EFT;
    public AudioSource stage02EFT;


    float doorYrotateValue = 0;
    float door2YrotateValue = 0;
    float door3YrotateValue = 0;

    public GameObject stage01ClearLight;
    public GameObject stage02ClearLight;

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
        stage01ClearLight.SetActive(false);
        stage02ClearLight.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //스테이지01 문열림
        if (listStage01.Count == 0 && doorYrotateValue >= -90f)
        {
            doorYrotateValue -= Time.deltaTime * 15f;

            stage01door.localEulerAngles = new Vector3(0, doorYrotateValue, 0);

            stage01ClearLight.SetActive(true);

            stage01EFT.PlayOneShot(stage01EFT.clip);

        }

        //스테이지02 문열림
        if (listStage02_1.Count == 0 && listStage02_2.Count == 0)
        {
            if (door2YrotateValue >= -90f)
            {
                door2YrotateValue -= Time.deltaTime * 15f;

                stage02doorL.localEulerAngles = new Vector3(0, door2YrotateValue,0);
                stage02EFT.PlayOneShot(stage02EFT.clip);
                stage02ClearLight.SetActive(true);


            }
            if (door3YrotateValue <= 90f)
            {
                door3YrotateValue += Time.deltaTime * 15f;

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

        for (int i = 0; i < listStage02_1.Count; i++)
        {
            if (enemy == listStage02_1[i])
            {
                listStage02_1.RemoveAt(i);
            }
        }

        for (int i = 0; i < listStage02_2.Count; i++)
        {
            if (enemy == listStage02_2[i])
            {
                listStage02_2.RemoveAt(i);
            }
        }
    }
}
