using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameDataManager : MonoBehaviour // information to keep between scenes but not after program quits.
{
    public string gameType;
    public string Player1ProfileName;
    public string Player1AllianceColor;
    public int Player1AllianceStation;
    public int player1ControllerId;
    public InputDevice player1Controller;
    public string Player2ProfileName;
    public string Player2AllianceColor;
    public int Player2AllianceStation;
    public int player2ControllerId;
    public InputDevice player2Controller;
    public static GameDataManager Instance;

    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        gameType = "none";
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
