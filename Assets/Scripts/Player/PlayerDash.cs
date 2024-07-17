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
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time > nextDashTime)
        {
            StartDash();
        }

        if (isDashing)
        {
            PerformDash();
        }
    }

    void StartDash()
    {
        Vector3 dashDir = playerScript.dir;
        dashDir.y = 0;

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

        Application.targetFrameRate = 20;
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
}

