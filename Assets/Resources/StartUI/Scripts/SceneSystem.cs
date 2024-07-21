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
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "MapScene")
        {

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

        }
    }
    public void GameStart()
    {
        SceneManager.LoadScene("MapScene");
    }
    public void GameOver()
    {
        
            SceneManager.LoadScene("02.GameOver");
        
    }
    public void GameClear()
    {

        SceneManager.LoadScene("03.GameClear");


    }

}
