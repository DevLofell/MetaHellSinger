using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public Transform target;

    public float rotSpeed = 300f;
    float mx = 0;
    float my = 0;
    void Start()
    {
    }

    void LateUpdate()
    {
        CamRotate();
    }

    void CamRotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");


        mx += mouseX * rotSpeed * Time.deltaTime;
        my += mouseY * rotSpeed * Time.deltaTime;

        my = Mathf.Clamp(my, -60f, 88f);


        transform.eulerAngles = new Vector3(-my, mx, 0);

    }

    
}
