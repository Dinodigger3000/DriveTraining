using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScreen : MonoBehaviour
{
    public Transform mainMenu;
    public Transform optionsMenu;
    public Transform playLocalMenu;
    public Transform playSoloMenu;
    void Awake() {
        OpenMainMenu("Start");
    }
    public void OpenMainMenu(string previousMenu) {
        mainMenu.gameObject.SetActive(true);
        mainMenu.Find("MainButtons").gameObject.SetActive(true);
        mainMenu.Find("PlayButtons").gameObject.SetActive(false);

        playLocalMenu.gameObject.SetActive(false);
        playSoloMenu.gameObject.SetActive(false);
        optionsMenu.gameObject.SetActive(false);
        if (previousMenu == "PlayModeSelect") {
            mainMenu.Find("MainButtons/PlayButton").gameObject.GetComponent<Button>().Select();
        }
        if (previousMenu == "Options") {
            mainMenu.Find("MainButtons/OptionsButton").gameObject.GetComponent<Button>().Select();
        }
        if (previousMenu == "Start") {
            mainMenu.Find("MainButtons/PlayButton").gameObject.GetComponent<Button>().Select();
        }
    }
    public void OpenOptions() {
        mainMenu.gameObject.SetActive(false);
        optionsMenu.gameObject.SetActive(true);
        optionsMenu.Find("BackButton").gameObject.GetComponent<Button>().Select();
    }
    public void OpenPlayModeMenu() {
        mainMenu.Find("MainButtons").gameObject.SetActive(false);
        mainMenu.Find("PlayButtons").gameObject.SetActive(true);
        mainMenu.Find("PlayButtons/BackButton").gameObject.GetComponent<Button>().Select();
    }
    public void OpenPlaySoloMenu() {
        mainMenu.gameObject.SetActive(false);
        playSoloMenu.gameObject.SetActive(true);
        playSoloMenu.Find("BackButton").gameObject.GetComponent<Button>().Select();
    }
    public void OpenPlayLocalMenu() {
        mainMenu.gameObject.SetActive(false);
        playLocalMenu.gameObject.SetActive(true);
        playLocalMenu.Find("BackButton").gameObject.GetComponent<Button>().Select();
    }
    public void PlayGame() {  
        SceneManager.LoadScene("NewField");  
    }  
    public void QuitGame() {  
        Debug.Log("QUIT");  
        SceneManager.LoadScene("Quit");  
    }  
    void Start() {}
    void Update() {}
}
