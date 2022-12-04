using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class SingleplayerManager : MonoBehaviour
{
    StationManager stationManager;
    Station station;
    private PlayerInput inputManager;
    public string allianceColor;
    public int allianceStation;
    public GameObject playerPrefab;
    public GameObject player;
    void Start()
    {
        
        allianceColor = GameDataManager.Instance.Player1AllianceColor;
        allianceStation = GameDataManager.Instance.Player1AllianceStation;
        if (allianceColor == "Red") {
            station = stationManager.FindStation("R"+allianceStation);
        } else {
            station = stationManager.FindStation("B"+allianceStation);
        }
        station.Activate();
        player = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        inputManager = player.GetComponent("PlayerInput") as PlayerInput;
        player.GetComponent<PlayerDetails>().spawn = station.spawn;
        player.GetComponent<PlayerDetails>().playerID = inputManager.playerIndex;
        inputManager.camera = station.GetCamera();
        Debug.Log(allianceColor+", "+allianceStation+", "+inputManager.playerIndex);
    }
    void Awake()
    {
        stationManager = GameObject.Find("StationManager").GetComponent("StationManager") as StationManager;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
