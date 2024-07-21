using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerDash : MonoBehaviour
{
    public Player playerScript;

    public CharacterController characterController;
    public float dashDistance = 5f;
    public float dashCooldown = 1f;
    public float dashDuration = 0.2f;

    private float nextDashTime;
    private Vector3 dashDirection;
    private bool isDashing;
    private float dashStartTime;

    PostProcessVolume ppv;
    MotionBlur dasheEffect;

    public Animator wingAni;


    private void Start()
    {
        ppv = Camera.main.GetComponent<PostProcessVolume>();
        //vignetteEffect = ppv.GetComponentInChildren<Vignette>();

        if (ppv.profile.TryGetSettings(out dasheEffect))
        {
            // Vignette 초기 설정
            dasheEffect.active = false;
        }
    }

    void Update()
    {

        WingAni();
        if (isDashing)
        {
            PerformDash();
        }
    }

    void StartDash()
    {
        Vector3 dashDir = playerScript.dir;
        dashDir.y = 0;


        //대쉬 소리
        SoundManager.instance.PlayEftSound(SoundManager.ESoundType.EFT_Dash);

        if (dashDir.magnitude == 0)
        {
            dashDirection = playerScript.transform.forward;
        }
        else
        {
            dashDirection = playerScript.dir;
            dashDirection.y = 0;

        }

        dashStartTime = Time.time;
        nextDashTime = Time.time + dashCooldown;
        isDashing = true;

        Application.targetFrameRate = 30;
        dasheEffect.active = true;

    }

    void PerformDash()
    {
        float dashElapsed = Time.time - dashStartTime;
        if (dashElapsed < dashDuration)
        {
            characterController.Move(dashDirection * (dashDistance / dashDuration) * Time.deltaTime);
        }
        else
        {
            isDashing = false;
            dasheEffect.active = false;
            Application.targetFrameRate = 60;
        }
    }

    void WingAni()
    {
        if (Input.GetKey(KeyCode.A) && Input.GetKeyDown(KeyCode.LeftShift) && Time.time > nextDashTime)
        {
            wingAni.SetTrigger("OnDashL");
            StartDash();
        }
        else if (Input.GetKey(KeyCode.D) && Input.GetKeyDown(KeyCode.LeftShift) && Time.time > nextDashTime)
        {
            wingAni.SetTrigger("OnDashR");
            StartDash();
        }
        else if (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.LeftShift) && Time.time > nextDashTime)
        {
            wingAni.SetTrigger("OnDashB");
            StartDash();
        }
        else if(Input.GetKeyDown(KeyCode.LeftShift) && Time.time > nextDashTime)
        {
            //print("대쉬");
            StartDash();
        }
    }
}

