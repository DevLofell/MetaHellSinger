using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public Transform target;

    public float rotSpeed = 200f;
    float mx = 0;
    float my = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CamRotate();
        CamFollow();
    }

    void CamRotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        mx += mouseX * rotSpeed * Time.deltaTime;
        my += mouseY * rotSpeed * Time.deltaTime;

        my = Mathf.Clamp(my, -88f, 88f);


        transform.eulerAngles = new Vector3(-my, mx, 0);

    }

    void CamFollow()
    {
        transform.position = target.position;
    }
}
