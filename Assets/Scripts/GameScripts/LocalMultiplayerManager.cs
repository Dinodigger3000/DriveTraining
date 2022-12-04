using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
public class LocalMultiplayerManager : MonoBehaviour
{
    private PlayerInputManager inputManager;
    InputDevice p1Device;
    InputDevice p2Device;
    void Start()
    {
        inputManager = GameObject.Find("PlayerManager").GetComponent("PlayerInputManager") as PlayerInputManager;
    
        p1Device = GameDataManager.Instance.player1Controller;
        p2Device = GameDataManager.Instance.player2Controller;
        inputManager.JoinPlayer(1, 1, null, p1Device);
        inputManager.JoinPlayer(2, 2, null, p2Device);
    
    }
    void Update()
    {
        
    }
}
