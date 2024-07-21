using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class SceneSystem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameOver();
    }
    public void GameStart()
    {
        SceneManager.LoadScene("MapScene");
    }
    public void GameOver()
    {
        if(Player.instance.currHP <= 0)
        {
            SceneManager.LoadScene("02.GameOver");
        }
    }
    public void GameClear()
    {
        //만약 보스가 죽으면

        //클리어 씬이 나온다.
    }
}
