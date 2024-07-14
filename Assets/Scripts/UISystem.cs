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

        //hp¹Ù
        hpbar.fillAmount = Mathf.Lerp(hpbar.fillAmount, Player.instance.currHP / Player.instance.maxHP, 10 * Time.deltaTime);

        //mp¹Ù
        if (playerAni.GetInteger("weaponState") == 0)
        {

            mpbar.fillAmount = Mathf.Lerp(mpbar.fillAmount, Player.instance.currSwordMP / Player.instance.maxMP, 10 * Time.deltaTime);
        }
        else if (playerAni.GetInteger("weaponState") == 1)
        {

            mpbar.fillAmount = Mathf.Lerp(mpbar.fillAmount, Player.instance.currSkullMP / Player.instance.maxMP, 10 * Time.deltaTime);
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
