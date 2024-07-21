using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class SceneSystem : MonoBehaviour
{
    public static SceneSystem instance;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

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
        if (Player.instance.currHP <= 0)
        {
            SceneManager.LoadScene("02.GameOver");
        }
    }
    public void GameClear()
    {

        SceneManager.LoadScene("03.GameClear");


    }

}
