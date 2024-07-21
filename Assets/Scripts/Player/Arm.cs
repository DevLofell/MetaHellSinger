using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : MonoBehaviour
{
    public Animator playerAini;

    public Transform tr;

    public bool armMovePlay;

    Vector3 rot;
    Vector3 rot2;
    Vector3 ori;


    bool armRot;
    // Start is called before the first frame update
    void Start()
    {
        armRot = true;
        armMovePlay = false;
        rot2 = rot = tr.localEulerAngles;
        ori = tr.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {

        ArmMoving();
        ArmReset();
    }

    float n = -1;
    void ArmMoving()
    {
        if (armMovePlay)
        {
            if (n == -1)
            {
                rot2 = tr.localEulerAngles;
            }
            else
            {
                // ���� �������� rot2 �� ����.
                rot2 = rot;

            }

            Vector3 ���� = tr.localEulerAngles;
            n = 0;
            if (armRot)
            {
                rot = ���� + new Vector3(20, 10, -20);
                armRot = false;
            }
            else if (!armRot)
            {
                rot = ���� + new Vector3(-20, -10, 20);
                armRot = true;
            }
            armMovePlay = false;
        }

        if (n != -1)
        {
            n += Time.deltaTime * 10;
            if (n > 1) n = 1;
            tr.localEulerAngles = Vector3.Lerp(rot2, rot, n);
            //tr.localEulerAngles += rot;

        }
    }

    void ArmReset()
    {
        if (playerAini.GetInteger("weaponState") != 1)
        {
            tr.localEulerAngles = ori;

        }
    }
}
