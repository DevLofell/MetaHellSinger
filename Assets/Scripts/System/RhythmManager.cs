using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;
using Debug = UnityEngine.Debug;
using UnityEngine.UI;


public class RhythmManager : MonoBehaviour
{

    //�ð� üũ�� �ϰ� �ʹ�.
    //���� ���� �ð� �� Ÿ�̹� üũ
    //
    Stopwatch sw;


    public Image imagefactory;
    public RectTransform imageRT;
    public Slider slider;

    

    // Start is called before the first frame update
    void Start()
    {
        if(sw == null)
        {
            sw = new Stopwatch();
        }
        sw.Start();


        Vector3[] localCorners = new Vector3[4];
        imageRT.GetLocalCorners(localCorners);


        RectTransform imageRectTransform = imageRT.transform as RectTransform;
        imageRectTransform.position = localCorners[0];

    }

    // Update is called once per frame
    void Update()
    {
       

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            
        }
    }
    
    void TimeCheck()
    {
        Debug.Log(sw.Elapsed.ToString());
        sw.Stop();
        sw.Reset();
        sw.Start();
    }

    void LerpCheck()
    {
        //�̹����� ��𿡼� ������ �Է��ϰ� �ʹ�.
        //1. �̹��� ���� ��ġ ��
        //2. �̹��� �� ��ġ ��
   
       /* RectTransform rect = imageRT.transform as RectTransform;
        Vector3[] worldCorners = new Vector3[4];
        rect.GetWorldCorners(worldCorners);
        
        Vector2 
        
        float ImageStartPos = rect.sizeDelta.x
        
        
        GameObject go = GameObject.Instantiate(imagefactory.gameObject,imageRT.transform, );*/
        
    }


}
