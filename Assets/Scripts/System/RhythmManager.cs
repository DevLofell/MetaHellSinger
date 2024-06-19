using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;
using Debug = UnityEngine.Debug;


public class RhythmManager : MonoBehaviour
{

    //시간 체크를 하고 싶다.
    //음악 시작 시간 및 타이밍 체크
    //
    Stopwatch sw;
    

    // Start is called before the first frame update
    void Start()
    {
        if(sw == null)
        {
            sw = new Stopwatch();
        }
        sw.Start();
    }

    // Update is called once per frame
    void Update()
    {
        

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log(sw.Elapsed.ToString());
            sw.Stop();
            sw.Reset();
            sw.Start();
        }
    }

}
