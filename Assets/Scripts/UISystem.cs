using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISystem : MonoBehaviour
{
    public Animator playerAni;
    public GameObject swordState;
    public GameObject skullState;

    public Image hpbar;
    public Image mpbar;


    // Start is called before the first frame update
    void Start()
    {
        WeaponChange();

        swordState.SetActive(false);
    }

    // Update is called once per frame
    void Update()
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

        //hp��
        hpbar.fillAmount = Mathf.Lerp(hpbar.fillAmount, Player.instance.currHP / Player.instance.maxHP, 10 * Time.deltaTime);

        //mp��
        mpbar.fillAmount = Mathf.Lerp(mpbar.fillAmount, Player.instance.currMP / Player.instance.maxMP, 10 * Time.deltaTime);

    }

    public void UpdateHP(float value)
    {
        // ���� HP�� value ������.
        Player.instance.currHP += value;

        // ���� HP�� 0�̸�
        if (Player.instance.currHP <= 0)
        {
            // �ı�����
            //Destroy(gameObject);
        }
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
}
