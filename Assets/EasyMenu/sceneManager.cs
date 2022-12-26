using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour
{
    #region Singleton
    public static sceneManager Instance { get; private set; }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion


    public void nextScene()
    {
       LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);

   
    }

    public void reload()
    {
        
            LoadLevel(SceneManager.GetActiveScene().buildIndex);
           
    }

    public void LoadLevel(int index)
    {
        SceneManager.LoadScene(index);
    }


    public void Quit()
    {
        print("quitting");
        Application.Quit();
    }


    public void pause()
    {
        Time.timeScale = 0;
    }

    public void unPause()
    {
        Time.timeScale = 1;
    }
}
