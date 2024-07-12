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

        //hp바
        hpbar.fillAmount = Mathf.Lerp(hpbar.fillAmount, Player.instance.currHP / Player.instance.maxHP, 10 * Time.deltaTime);

        //mp바
        mpbar.fillAmount = Mathf.Lerp(mpbar.fillAmount, Player.instance.currMP / Player.instance.maxMP, 10 * Time.deltaTime);

    }

    public void UpdateHP(float value)
    {
        // 현재 HP를 value 더하자.
        Player.instance.currHP += value;

        // 현재 HP가 0이면
        if (Player.instance.currHP <= 0)
        {
            // 파괴하자
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
