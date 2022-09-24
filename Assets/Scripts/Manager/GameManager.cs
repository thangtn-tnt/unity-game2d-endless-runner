using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    #region Collectable info
    
    public int coin;
    
    #endregion


    [SerializeField] private string beginScene;
    
    void Awake()
    {
        instance = this; 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            gameRestart();
        }
    }

    public void gameRestart()
    {
        SceneManager.LoadScene(beginScene);
    }

}
