using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class UISystem : MonoBehaviour
{
    public Player player;

    public Animator playerAni;
    public GameObject swordState;
    public GameObject skullState;
    public GameObject swordStateSkill;
    public GameObject skullStateSkill;

    public Image hpbar;
    public Image mpbar;

    public GameObject SkillEffectUI;
    public GameObject DeathEffectUI;
    PostProcessVolume ppv;
    Vignette vignetteEffect;

    public GameObject stage01Mision;
    public GameObject stage02Mision;
    public GameObject bossMision;

    public Text stage01MiZombi;
    public Text stage02MiZombi;
    public Text stage02Miminidragon;







    void Start()
    {
        ppv = Camera.main.GetComponent<PostProcessVolume>();
        //vignetteEffect = ppv.GetComponentInChildren<Vignette>();

        if (ppv.profile.TryGetSettings(out vignetteEffect))
        {
            // Vignette �ʱ� ����
            vignetteEffect.intensity.value = 0.0f; // ����
            vignetteEffect.smoothness.value = 1.0f; // �ε巯��
            vignetteEffect.roundness.value = 1.0f; // �ձ۱�
        }

        stage01Mision.SetActive(true);
        stage02Mision.SetActive(true);
        bossMision.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        WeaponChange();

        HPMP();

        ParticleUI();

        MonsterCheck();
    }



    void WeaponChange()
    {
        if (playerAni.GetInteger("weaponState") == 1)
        {
            swordState.SetActive(false);
            skullState.SetActive(true);
        }
        else
        {
            swordState.SetActive(true);
            skullState.SetActive(false);
        }
    }
    void HPMP()
    {
        //hp��
        hpbar.fillAmount = Mathf.Lerp(hpbar.fillAmount, player.currHP / player.maxHP, 10 * Time.deltaTime);

        //mp��
        if (playerAni.GetInteger("weaponState") == 0 || playerAni.GetInteger("weaponState") == 2)
        {

            mpbar.fillAmount = Mathf.Lerp(mpbar.fillAmount, player.currSwordMP / player.maxMP, 10 * Time.deltaTime);
        }
        else if (playerAni.GetInteger("weaponState") == 1)
        {

            mpbar.fillAmount = Mathf.Lerp(mpbar.fillAmount, player.currSkullMP / player.maxMP, 10 * Time.deltaTime);
        }
    }
    void ParticleUI()
    {
        //ü���� 30�� ������ ��
        if (Player.instance.currHP / Player.instance.maxHP <= 0.3f)
        {
            DeathEffectUI.SetActive(true);
            if (vignetteEffect.intensity.value < 0.3f)
            {
                vignetteEffect.intensity.value += 0.01f;
            }
            else
            {

                vignetteEffect.intensity.value = 0.4f; // ����
            }
        }
        else
        {
            DeathEffectUI.SetActive(false);
            vignetteEffect.intensity.value = 0.0f; // ����


        }

        //���⺰ mp�� �ƽ��� ��
        if (playerAni.GetInteger("weaponState") == 0 && Player.instance.currSwordMP == Player.instance.maxMP)
        {
            SkillEffectUI.SetActive(true);
            swordStateSkill.SetActive(true);
            skullStateSkill.SetActive(false);

        }
        else if (playerAni.GetInteger("weaponState") == 1 && Player.instance.currSkullMP == Player.instance.maxMP)
        {
            SkillEffectUI.SetActive(true);
            swordStateSkill.SetActive(false);
            skullStateSkill.SetActive(true);


        }
        else
        {
            SkillEffectUI.SetActive(false);
            swordStateSkill.SetActive(false);
            skullStateSkill.SetActive(false);

        }
    }

    void MonsterCheck()
    {
        if(StageCheck.instance.listStage01.Count == 0)
        {
            stage01Mision.SetActive(false);
        }
        if (StageCheck.instance.listStage02_1.Count == 0 && StageCheck.instance.listStage02_2.Count == 0)
        {
            stage02Mision.SetActive(false);
            bossMision.SetActive(true);
        }
        stage01MiZombi.text = "���� (" + StageCheck.instance.listStage01.Count + "/9)";
        stage02MiZombi.text = "���� (" + StageCheck.instance.listStage02_2.Count + "/6)";
        stage02Miminidragon.text = "���� �巡�� (" + StageCheck.instance.listStage02_1.Count + "/3)";
    }

}

