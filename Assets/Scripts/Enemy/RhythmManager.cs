using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;
using Debug = UnityEngine.Debug;
using UnityEngine.UI;


public class RhythmManager : MonoBehaviour
{

    //시간 체크를 하고 싶다.
    //음악 시작 시간 및 타이밍 체크
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
        //이미지를 어디에서 어디까지 입력하고 싶다.
        //1. 이미지 시작 위치 값
        //2. 이미지 끝 위치 값
   
       /* RectTransform rect = imageRT.transform as RectTransform;
        Vector3[] worldCorners = new Vector3[4];
        rect.GetWorldCorners(worldCorners);
        
        Vector2 
        
        float ImageStartPos = rect.sizeDelta.x
        
        
        GameObject go = GameObject.Instantiate(imagefactory.gameObject,imageRT.transform, );*/
        
    }


}
