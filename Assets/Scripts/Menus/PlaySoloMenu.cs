using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoloMenu : MonoBehaviour
{
    public MenuScreen menuScreen;
    public string allianceColor = "Red";
    public int allianceStation = 0;
    public string profileName = "Player";
    public string subMenu;
    public ProfileSelectMenu RedProfileSelectMenu;
    public ProfileSelectMenu BlueProfileSelectMenu;
    public Transform RedGameSetupMenu;
    public Transform BlueGameSetupMenu;
    public void OpenProfileSelect() {
        RedGameSetupMenu.gameObject.SetActive(false);
        BlueGameSetupMenu.gameObject.SetActive(false);
        if (allianceColor == "Red") {
            RedProfileSelectMenu.gameObject.SetActive(true);
            BlueProfileSelectMenu.gameObject.SetActive(false);
            if (profileName != "Player") {
                RedProfileSelectMenu.SetPlayerProfile(profileName);
            }
        }
        if (allianceColor == "Blue") {
            BlueProfileSelectMenu.gameObject.SetActive(true);
            RedProfileSelectMenu.gameObject.SetActive(false);
            if (profileName != "Player") {
                BlueProfileSelectMenu.SetPlayerProfile(profileName);
            }
        }
        subMenu = "ProfileSelect";
    }
    public void OpenGameSetup() {
        RedProfileSelectMenu.gameObject.SetActive(false);
        BlueProfileSelectMenu.gameObject.SetActive(false);
        if (allianceColor == "Red") {
        RedGameSetupMenu.gameObject.SetActive(true);
        BlueGameSetupMenu.gameObject.SetActive(false);
        }
        if (allianceColor == "Blue") {
        RedGameSetupMenu.gameObject.SetActive(false);
        BlueGameSetupMenu.gameObject.SetActive(true);
        }
        SetStation(allianceStation);
        subMenu = "GameSetup";
    }
    public void SetAllianceColor(string color) {
        allianceColor = color;
        if (subMenu == "ProfileSelect") {
            OpenProfileSelect();
        } else if (subMenu == "GameSetup") {
            OpenGameSetup();
        }
        if (color == "Red") {
            transform.Find("TitlePlayer/TitleRed").gameObject.SetActive(true);
            transform.Find("TitlePlayer/TitleBlue").gameObject.SetActive(false);
        }
        else if (color == "Blue") {
            transform.Find("TitlePlayer/TitleBlue").gameObject.SetActive(true);
            transform.Find("TitlePlayer/TitleRed").gameObject.SetActive(false);
        }
    }
    public void SetAllianceStation(int newStation) {
        if (allianceStation == newStation) {
            SetStation(0);
        } else {
            SetStation(newStation);
        }
    }
    private void SetStation(int newStation) {
        if (allianceStation == 0 && newStation == 0) {
            return; // do nothing
        }
        if (allianceColor == "Red") {
            // deselect stations
            RedGameSetupMenu.Find("AllianceStationSelectWindow/"+1+"Selected").gameObject.SetActive(false);
            RedGameSetupMenu.Find("AllianceStationSelectWindow/"+2+"Selected").gameObject.SetActive(false);
            RedGameSetupMenu.Find("AllianceStationSelectWindow/"+3+"Selected").gameObject.SetActive(false);
            RedGameSetupMenu.Find("AllianceStationSelectWindow/"+1).gameObject.SetActive(true);
            RedGameSetupMenu.Find("AllianceStationSelectWindow/"+2).gameObject.SetActive(true);
            RedGameSetupMenu.Find("AllianceStationSelectWindow/"+3).gameObject.SetActive(true);
            if (newStation == 0) {
                allianceStation = 0;
                return;
            }
            // select new station
            RedGameSetupMenu.Find("AllianceStationSelectWindow/"+newStation+"Selected").gameObject.SetActive(true);
            RedGameSetupMenu.Find("AllianceStationSelectWindow/"+newStation).gameObject.SetActive(false); 
        } else if (allianceColor == "Blue") {
            // deselect stations
            BlueGameSetupMenu.Find("AllianceStationSelectWindow/"+1+"Selected").gameObject.SetActive(false);
            BlueGameSetupMenu.Find("AllianceStationSelectWindow/"+2+"Selected").gameObject.SetActive(false);
            BlueGameSetupMenu.Find("AllianceStationSelectWindow/"+3+"Selected").gameObject.SetActive(false);
            BlueGameSetupMenu.Find("AllianceStationSelectWindow/"+1).gameObject.SetActive(true);
            BlueGameSetupMenu.Find("AllianceStationSelectWindow/"+2).gameObject.SetActive(true);
            BlueGameSetupMenu.Find("AllianceStationSelectWindow/"+3).gameObject.SetActive(true);
            if (newStation == 0) {
                allianceStation = 0;
                return;
            }
            // select new station
            BlueGameSetupMenu.Find("AllianceStationSelectWindow/"+newStation+"Selected").gameObject.SetActive(true);
            BlueGameSetupMenu.Find("AllianceStationSelectWindow/"+newStation).gameObject.SetActive(false); 
        }
        allianceStation = newStation;
    }
    public void SetPlayerProfile(Transform button) {
        profileName = button.name;
        if (allianceColor == "Red") {
            RedProfileSelectMenu.SetPlayerProfile(profileName);
        } else if (allianceColor == "Blue") {
            BlueProfileSelectMenu.SetPlayerProfile(profileName);
        }
    }
    private void SetData() {
        GameDataManager.Instance.Player1ProfileName = profileName;
        GameDataManager.Instance.Player1AllianceColor = allianceColor;
        GameDataManager.Instance.Player1AllianceStation = allianceStation;
    }
    public void Continue() {
        if (subMenu == "ProfileSelect") {
            OpenGameSetup();
        } else if (subMenu == "GameSetup") {
            SetData();
            GameDataManager.Instance.gameType = "Solo";
            menuScreen.PlayGame();
        }
        SetData();
    }
    public void Back() {
        if (subMenu == "ProfileSelect") {
            menuScreen.OpenMainMenu("PlayModeSelect");
        } else if (subMenu == "GameSetup") {
            OpenProfileSelect();
        }
        SetData();
    }
    void Start() {
        OpenProfileSelect();
    }
    void OnEnable() {
        OpenProfileSelect();
    }
    void Update() {}
}
