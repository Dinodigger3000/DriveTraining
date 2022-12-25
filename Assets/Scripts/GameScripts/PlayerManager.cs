using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    StationManager stationManager;
    public void OnPlayerJoined(PlayerInput inputInstance) {
        Debug.Log("PlayerInput ID: " + inputInstance.playerIndex);
        Station assignedStation = stationManager.GetNextOpenStation();
        if (inputInstance.playerIndex == 1) {
            inputInstance.gameObject.GetComponent<PlayerDetails>().profileName = GameDataManager.Instance.Player1ProfileName;
            assignedStation = stationManager.FindStation("R2");
            assignedStation.GetCamera().rect = new Rect(0.0f, 0.5f, 1.0f, 0.5f);
        } else if (inputInstance.playerIndex == 2) {
            inputInstance.gameObject.GetComponent<PlayerDetails>().profileName = GameDataManager.Instance.Player2ProfileName;
            assignedStation = stationManager.FindStation("B2");
            assignedStation.GetCamera().rect = new Rect(0.0f, 0.0f, 1.0f, 0.5f);
        }

        inputInstance.camera = assignedStation.GetCamera();
        inputInstance.gameObject.GetComponent<PlayerDetails>().playerID = inputInstance.playerIndex;
        inputInstance.gameObject.GetComponent<PlayerDetails>().spawn = assignedStation.spawn;
        assignedStation.Activate();
    }
    void Awake()
    {
        stationManager = GameObject.Find("StationManager").GetComponent("StationManager") as StationManager;
        Debug.Log(stationManager.stations.ToString());
    }
    void Update()
    {
        
    }
}
