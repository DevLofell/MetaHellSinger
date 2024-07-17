using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Transform tr;
    Vector3 originRot;
    
    // Start is called before the first frame update
    void Start()
    {
        originRot = tr.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void LateUpdate()
    {
        originRot.x += 10;
        tr.localEulerAngles = originRot;
    }
}
